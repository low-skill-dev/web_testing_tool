using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using WebApi.Database;
using Microsoft.EntityFrameworkCore;
using SameSiteMode = Microsoft.AspNetCore.Http.SameSiteMode;
using Models.Database.Common;
using wtt_main_server_services;
using Models.ServicesSettings;
using wtt_main_server_helpers;
using Models.Database.Infrastructure;
using System.Security.Cryptography;
using static WebApi.Controllers.ResponseMessages;
using OtpNet;
using static Models.Constants.CookieNames;
using static Models.Constants.CookiePaths;
using Models.Application.Common;
using WebApi.Infrastructure;
using Models.Api;
using Models.Api.Auth.Responses;
using System.Text.Unicode;
using System.Text;
using CommonLibrary.Helpers;
using WebApi.Infrastructure.Authorization;
using ServicesLayer.Services;
using SharedServices;
using static WebApi.Extensions.HttpContextExtensions;


namespace WebApi.Controllers.v1;

/* Тут половина легаси из vdb_main_server,
 * половина новая.
 */

[ApiController]
[Route("api/v1/[controller]")]
[Consumes("application/json")]
[Produces("application/json")]
public sealed class AuthController : ControllerBase
{
	private readonly WttContext _dbContext;
	private readonly WttJwtService _jwtService;
	private readonly JwtServiceSettings _jwtSettings;
	private readonly AuthControllerSettings _settings;
	private readonly CookieOptions _jwtAccessCookieOptions;
	private readonly CookieOptions _jwtRefreshCookieOptions;
	private readonly ILogger<AuthController> _logger;

	public AuthController(WttContext context, WttJwtService jwtService, SettingsProviderService settings, ILogger<AuthController> logger)
	{
		_dbContext = context;
		_jwtService = jwtService;
		_settings = settings.AuthControllerSettings;
		_jwtSettings = settings.WttJwtServiceSettings;
		_logger = logger;

		_jwtRefreshCookieOptions = new CookieOptions()
		{
			Secure = true,
			HttpOnly = true,
			IsEssential = true,
			Path = JwtRefreshTokenCookiePathV1,
			SameSite = SameSiteMode.Strict,
			MaxAge = TimeSpan.FromSeconds(_jwtSettings.RefreshTokenLifespanSeconds),
		};

		_jwtAccessCookieOptions = new CookieOptions()
		{
			Secure = _jwtRefreshCookieOptions.Secure,
			HttpOnly = _jwtRefreshCookieOptions.HttpOnly,
			IsEssential = _jwtRefreshCookieOptions.IsEssential,
			SameSite = _jwtRefreshCookieOptions.SameSite,
			MaxAge = TimeSpan.FromSeconds(_jwtSettings.AccessTokenLifespanSeconds),
		};
	}

	#region basics

	[HttpGet]
	[JwtAuthorize]
	public IActionResult ValidateToken()
	{
		return Ok();
	}

	[HttpGet]
	[JwtAuthorize]
	[Route("sessions")]
	public async Task<IActionResult> GetMySessions()
	{
		var userGuid = HttpContext.GetAuthedUser()!.Guid;

		var tokens = await _dbContext.RefreshJwts
			.Where(x => x.UserGuid == userGuid).ToListAsync();

		return Ok(tokens);
	}

	[HttpPost]
	[JwtAnonymous]
	public async Task<IActionResult> Login([FromBody][Required] LoginRequest request)
	{
		var found = await _dbContext.Users.AsNoTracking()
			.FirstOrDefaultAsync(x => x.Email == request.Email);

		if(found is null || !PasswordHasher.ConfirmPassword(request.Password, found.PasswordHash, found.PasswordSalt))
			return Unauthorized(InvalidCredentials);

		if(found.TotpSecretKeyForSha512 is not null)
		{
			if(request.TotpCode is null)
				return this.Continue(TotpRequired);

			if(!CheckTotp(found, request.TotpCode))
				return Unauthorized(TotpDidNotMatch);
		}

		var ret = AddNewJwtPairToResponse(found);

		try
		{
			await _dbContext.SaveChangesAsync();
		}
		catch(Exception ex)
		{
			return Problem();
		}

		return Ok(ret);
	}

	[HttpPut]
	[JwtAnonymous]
	public async Task<IActionResult> Register([FromBody][Required] RegistrationRequest request)
	{
		if(await _dbContext.Users.AnyAsync(x => x.Email == request.Email))
			return await Login(new(request));

		var passHash = PasswordHasher.HashPassword(request.Password, out var passSalt);
		var geo = GeoIpHeadersHelper.GetInfoFromRequest(Request);
		var toAdd = new DbUser
		{
			Guid = Guid.NewGuid(),
			Email = request.Email,
			PasswordSalt = passSalt,
			PasswordHash = passHash,
			RegistrationCity = geo.City,
			RegistrationCountry = geo.Country,
			RegistrationIPAddress = geo.IpAddress,
			Role = Models.Enums.UserRoles.Regular,
		};

#if DEBUG
		var currentUsers = await _dbContext.Users.ToListAsync();
#endif

		_dbContext.Users.Add(toAdd);
		try
		{
			await _dbContext.SaveChangesAsync();
		}
		catch(Exception ex)
		{
			return Problem();
		}


		_logger.LogInformation($"Added new user. " +
			$"Email: '{toAdd.Email}'. Address: '{toAdd.RegistrationIPAddress}'.");

		return await Login(new LoginRequest(request));
	}

	[HttpPatch]
	[JwtAnonymous]
	public async Task<IActionResult> Refresh()
	{
		/* IRequestCookieCollection has a different indexer contract 
		 * than IDictionary<TKey,TValue>, as it will return null for 
		 * missing entries rather than throwing an Exception.
		 */
		var tkn = Request.Cookies[JwtRefreshTokenCookieName];
		if(tkn is null) return Unauthorized();

		var claims = _jwtService.ValidateRefreshJwt(tkn);
		if(claims is null) return Unauthorized();

		var foundUser = await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Guid == claims.UserGuid);
		if(foundUser is null) return Unauthorized();

		var foundToken = await _dbContext.RefreshJwts.AsTracking().FirstOrDefaultAsync(x => x.JtiSha512 == SHA512.HashData(claims.Jti));
		if(foundToken is null) return Unauthorized();

		if(foundToken.UserGuid != foundUser.Guid) return Unauthorized();

		var ret = AddNewJwtPairToResponse(foundUser, foundToken);
		await _dbContext.SaveChangesAsync();
		return Ok(ret);
	}

	[HttpDelete]
	[JwtAuthorize]
	public async Task<IActionResult> TerminateSession([FromQuery] string? jtiShaHex, [FromServices] JwtMinIatStorage minIatStorage)
	{
		var tkn = Request.Cookies[JwtRefreshTokenCookieName];
		if(tkn is null) return Unauthorized();

		var claims = _jwtService.ValidateRefreshJwt(tkn);
		if(claims is null) return Unauthorized();

		var fromCookieJtiHash = SHA512.HashData(claims.Jti);
		var foundCookieJti = await _dbContext.RefreshJwts.FirstOrDefaultAsync(x => x.UserGuid == claims.UserGuid && x.JtiSha512.SequenceEqual(fromCookieJtiHash));
		if(foundCookieJti is null) return Unauthorized();

		if(string.IsNullOrWhiteSpace(jtiShaHex))
		{
			_dbContext.RefreshJwts.Remove(foundCookieJti);
			await _dbContext.SaveChangesAsync();
			return Ok();
		}

		var sessionAge = DateTime.UtcNow.Subtract(foundCookieJti.OriginIssuedAt);
		if(sessionAge.TotalDays < 3)
		{
			Response.Headers.RetryAfter = Convert.ToInt32(sessionAge.TotalSeconds).ToString();
			return this.ServiceUnavailable(TooYoungSession);
		}

		var targetJtiHash = SHA512.HashData(Convert.FromHexString(jtiShaHex));
		var foundTarget = await _dbContext.RefreshJwts.FirstOrDefaultAsync(x => x.UserGuid == claims.UserGuid && x.JtiSha512.SequenceEqual(targetJtiHash));
		if(foundTarget is null) return NotFound();


		_dbContext.RefreshJwts.Remove(foundTarget);
		await _dbContext.SaveChangesAsync();

		minIatStorage.SetObject(foundCookieJti.Guid, DateTime.UtcNow, TimeSpan.FromSeconds(_jwtSettings.AccessTokenLifespanSeconds));
		return Ok();
	}

	[HttpPatch]
	[JwtAuthorize]
	[Route("password")]
	public async Task<IActionResult> ChangePassword([FromBody][Required] ChangePasswordRequest request)
	{
		var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Guid == HttpContext.GetAuthedUser()!.Guid);
		if(user is null) return Unauthorized();

		SetPassword(user, request);

		await _dbContext.SaveChangesAsync();
		return Ok();
	}

	#endregion

	#region recovery

	private const string isRecoveryJwtClaim = nameof(isRecoveryJwtClaim);
	private const string emailJwtClaim = nameof(emailJwtClaim);
	private const string entropyClaim = nameof(entropyClaim);

	[HttpPut]
	[JwtAnonymous]
	[Route("recovery/{email}")]
	public async Task<IActionResult> CreateAndSendLink(
		[FromRoute][Required][EmailAddress] string email,
		[FromServices] EmailSendingService sender)
	{
		var found = await _dbContext.Users.FirstOrDefaultAsync(x => x.Email == email);
		if(found is null) return NotFound();

		var jwt = _jwtService.GenerateRecoveryJwt(found, out var jti);

		var now = DateTime.UtcNow;
		var geoInfo = GeoIpHeadersHelper.GetInfoFromRequest(Request);
		_dbContext.RecoveryJwts.Add(new()
		{
			JtiSha512 = SHA512.HashData(jti),
			City = geoInfo.City,
			Country = geoInfo.Country,
			IPAddress = geoInfo.IpAddress,
			IssuedAt = now,
			OriginIssuedAt = now,
			UserGuid = found.Guid,
		});

		await _dbContext.SaveChangesAsync();
		var result = (int)await sender.Send(new()
		{
			From = _jwtSettings.RecoveryEmailSendFromAddress,
			To = found.Email,
			Subject = "Password reset",
			FromName = "Web Testing Tool",
			Body = $"Your password reset link: {_settings.RecoveryEndpointWithoutToken}{jwt}"
		});

		return 200 <= result && result <= 299 ? Ok() : Problem();
	}

	[HttpPost]
	[JwtAnonymous]
	[Route("recovery/{jwt}")]
	public async Task<IActionResult> ResetPassword([FromRoute][Required] string jwt, [FromBody][Required] ChangePasswordRequest request)
	{
		if(string.IsNullOrWhiteSpace(jwt)) return BadRequest();

		var parsed = _jwtService.ValidateRefreshJwt(jwt);
		if(parsed is null) return Unauthorized();

		var jtiFromDb = await _dbContext.RecoveryJwts.FirstOrDefaultAsync(x => x.JtiSha512.SequenceEqual(SHA512.HashData(parsed.Jti)));
		if(jtiFromDb is null) return Unauthorized();

		var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Guid == jtiFromDb.UserGuid);
		if(user is null) return Unauthorized();

		SetPassword(user, request);
		_dbContext.RecoveryJwts.Remove(jtiFromDb);
		await _dbContext.SaveChangesAsync();

		return Ok();
	}

	#endregion

	#region NonAction

	/// <summary>
	/// For specified user model, issues access JWT token with public user info.<br/>
	/// If required, also provides refresh token, which's JTI SHA512 hash is written<br/>
	/// into the Db. The originIssuedAt parameter indicated when the very first token<br/>
	/// of the series was issued. If users logging in, the series is started, all<br/>
	/// refresh token made after this still keeps the data where the first token was<br/>
	/// issued.
	/// </summary>
	[NonAction]
	public JwtResponse AddNewJwtPairToResponse(DbUser user, RefreshJwt? baseToken = null)
	{
		var accessTkn = _jwtService.GenerateAccessJwt(user);
		var refreshTkn = _jwtService.GenerateRefreshJwt(user, out var jti);

		Response.Cookies.Append(JwtAccessTokenCookieName, accessTkn, _jwtAccessCookieOptions);
		Response.Cookies.Append(JwtRefreshTokenCookieName, refreshTkn, _jwtRefreshCookieOptions);

		const string fakeSig =
			@"AfzSpxGfNSJUPEEdl7ezvpYU4OFUiKMarpNMoxlFv01RvgGJ_e5L592Z7PnSajT7NV2n-UDnx3pJEzQECmKvQZ9" +
			@"MAFeKzSBYv6GN2lR88WS-w9cfqqEDSGBEa_UrxQbZxfNh7kcXdnivEGBxW9XkLSm3PDGGHfcozmYAkh3kWij53I8t";
		var ret = new JwtResponse
		{
			// With fake signature, allows JS to read the payload
			Access = string.Join('.', accessTkn.Split('.').Take(2).Append(fakeSig)),
			Refresh = string.Join('.', refreshTkn.Split('.').Take(2).Append(fakeSig)),
		};

		var now = DateTime.UtcNow;
		var geoInfo = GeoIpHeadersHelper.GetInfoFromRequest(Request);
		var dbRefresh = new RefreshJwt()
		{
			JtiSha512 = SHA512.HashData(jti),
			UserGuid = user.Guid,
			IPAddress = geoInfo.IpAddress,
			City = geoInfo.City,
			Country = geoInfo.Country,
			IssuedAt = now,                                     // do not use DateTime.UtcNow twice here
			OriginIssuedAt = baseToken?.OriginIssuedAt ?? now,  // we need exactly same values
		};

		if(baseToken is not null) _dbContext.RefreshJwts.Remove(baseToken);
		_dbContext.RefreshJwts.Add(dbRefresh);
		return ret;
	}

	[NonAction]
	private void SetPassword(DbUser dbEntry, ChangePasswordRequest request)
	{
		var passHash = PasswordHasher.HashPassword(request.Password, out var passSalt);
		dbEntry.PasswordSalt = passSalt;
		dbEntry.PasswordHash = passHash;
	}

	[NonAction]
	public bool CheckTotp(DbUser user, string code)
	{
		var totp = new Totp(user.TotpSecretKeyForSha512, mode: OtpHashMode.Sha512);

		var now = DateTime.UtcNow;
		var prev = totp.ComputeTotp(now.AddSeconds(-30));
		var curr = totp.ComputeTotp(now);
		var next = totp.ComputeTotp(now.AddSeconds(30));

		return prev == code || curr == code || next == code;
	}

	#endregion
}