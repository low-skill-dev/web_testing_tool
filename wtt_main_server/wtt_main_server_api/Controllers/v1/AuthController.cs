using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using webooster.Services;
using wtt_main_server_api.Database;
using Microsoft.EntityFrameworkCore;
using SameSiteMode = Microsoft.AspNetCore.Http.SameSiteMode;
using wtt_main_server_data.Database.Common;
using wtt_main_server_services;
using wtt_main_server_data.ServicesSettings;
using wtt_main_server_helpers;
using wtt_main_server_data.Database.Infrastructure;
using System.Security.Cryptography;
using webooster.Helpers;
using static wtt_main_server_api.Controllers.ResponseMessages;
using OtpNet;
using static wtt_main_server_data.Constants.CookieNames;
using static wtt_main_server_data.Constants.CookiePaths;
using wtt_main_server_data.Application.Common;
using wtt_main_server_api.Infrastructure;
using wtt_main_server_data.Api;
using wtt_main_server_data.Api.Auth.Responses;
using System.Text.Unicode;
using System.Text;
using IdentityModel;

namespace wtt_main_server_api.Controllers.v1;

/* Тут половина легаси из vdb_main_server,
 * половина новая.
 */

[ApiController]
[Consumes("application/json")]
[Produces("application/json")]
[Route("api/v1/[controller]")]
public sealed class AuthController : ControllerBase
{
	private static readonly string _publicJwtSignature =
		Base64Url.Encode(SHA512.HashData(Encoding.UTF8.GetBytes("""
			Lorem ipsum dolor sit amet, consectetur adipiscing elit, 
			sed do eiusmod tempor incididunt ut labore et dolore magna 
			aliqua. Ut enim ad minim veniam, quis nostrud exercitation 
			ullamco laboris nisi ut aliquip ex ea commodo consequat. 
			Duis aute irure dolor in reprehenderit in voluptate velit 
			esse cillum dolore eu fugiat nulla pariatur. Excepteur sint 
			occaecat cupidatat non proident, sunt in culpa qui officia 
			deserunt mollit anim id est laborum.
		""")));

	private readonly WttContext _context;
	private readonly WttJwtService _jwtService;
	private readonly AuthControllerSettings _settings;
	private readonly ILogger<AuthController> _logger;
	private readonly CookieOptions _jwtAccessCookieOptions;
	private readonly CookieOptions _jwtRefreshCookieOptions;

	public AuthController(WttContext context, WttJwtService jwtService, AuthControllerSettings settings, ILogger<AuthController> logger)
	{
		this._context = context;
		this._jwtService = jwtService;
		this._settings = settings;
		this._logger = logger;

		_jwtRefreshCookieOptions = new CookieOptions()
		{
			Secure = true,
			HttpOnly = true,
			IsEssential = true,
			Path = JwtRefreshTokenCookiePath,
			SameSite = SameSiteMode.Strict,
			MaxAge = TimeSpan.FromSeconds(settings.RefreshTokenLifespanSeconds),
		};

		_jwtAccessCookieOptions = new CookieOptions()
		{
			Secure = _jwtRefreshCookieOptions.Secure,
			HttpOnly = _jwtRefreshCookieOptions.HttpOnly,
			IsEssential = _jwtRefreshCookieOptions.IsEssential,
			//Path = JwtAccessTokenCookiePath,
			SameSite = _jwtRefreshCookieOptions.SameSite,
			MaxAge = TimeSpan.FromSeconds(settings.AccessTokenLifespanSeconds),
		};
	}

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
	public async Task<JwtResponse> IssueJwtAndAddToUser(DbUser user, bool provideRefresh = true, DbJwtIdentifier? baseToken = null)
	{
		var accessTkn = this._jwtService.GenerateAccessJwt(user,
			TimeSpan.FromSeconds(_settings.AccessTokenLifespanSeconds));

		this.Response.Cookies.Append(JwtAccessTokenCookieName,
			accessTkn, _jwtAccessCookieOptions);

		var ret = new JwtResponse
		{
			// Creates fake signature but still allows JS
			// on the client side to read the payload
			Access = string.Join('.', accessTkn.Split('.').Take(2).Append("X"))
		};

		if(!provideRefresh) return ret;

		var jti = System.Security.Cryptography.RandomNumberGenerator.GetBytes(512 / 8);
		var geoInfo = GeoIpHeadersHelper.GetInfoFromRequest(this.Request);

		var refreshTkn = this._jwtService.GenerateRefreshJwt(jti, user.Guid,
			TimeSpan.FromSeconds(_settings.AccessTokenLifespanSeconds));

		this.Response.Cookies.Append(JwtRefreshTokenCookieName,
			refreshTkn, _jwtRefreshCookieOptions);

		ret.Refresh = string.Join('.', refreshTkn.Split('.').Take(2).Append("X"));

		var now = DateTime.UtcNow;
		var dbRefresh = new DbJwtIdentifier()
		{
			JtiSha512 = SHA512.HashData(jti), 
			RelatedUserGuid = user.Guid,
			IPAddress = geoInfo.IpAddress,
			City = geoInfo.City,
			Country = geoInfo.Country,
			IssuedAt = now,                                     // do now use DateTime.UtcNow twice here
			OriginIssuedAt = baseToken?.OriginIssuedAt ?? now,  // we need exactly same values
		};

		if(baseToken is not null) _context.RefreshJtis.Remove(baseToken);
		_context.RefreshJtis.Add(dbRefresh);
		await _context.SaveChangesAsync();

		return ret;
	}

	[NonAction]
	private void SetPassword(DbUser dbEntry, ChangePasswordRequest request)
	{
		var passHash = PasswordHelper.HashPassword(request.Password, out var passSalt);
		dbEntry.PasswordSalt = passSalt;
		dbEntry.PasswordHash = passHash;
	}

	[NonAction]
	public async Task<DbJwtIdentifier?> FindRefreshTokenInDb(string refreshJwt)
	{
		var parsed = _jwtService.ValidateRefreshJwt(refreshJwt);
		if(!parsed.HasValue) return null;

		var jtiHash = SHA512.HashData(parsed.Value.Jti);
		var found = await _context.RefreshJtis.FirstOrDefaultAsync(x => x.JtiSha512 == jtiHash);
		return found;
	}

	[NonAction]
	public bool CheckTotp(DbUser user, string providedCode)
	{
		var totp = new Totp(user.TotpSecretKeyForSha512, 30, OtpHashMode.Sha512, 6);

		if(totp.RemainingSeconds() < 5)
		{
			var code = totp.ComputeTotp(DateTime.UtcNow.AddSeconds(-30));
			if(code.Equals(providedCode)) return true;
		}

		return totp.ComputeTotp().Equals(providedCode);
	}

	#endregion

	[HttpGet]
	[JwtAuthorize]
	public IActionResult ValidateToken() => this.Ok();

	[HttpGet]
	[JwtAuthorize]
	[Route("sessions")]
	public async Task<IActionResult> GetMySessions()
	{
		var tokens = await this._context.RefreshJtis
			.Where(x => x.RelatedUserGuid == this.ParseGuidClaim())
			.ToListAsync();

		return this.Ok(tokens);
	}

	[HttpPost]
	[AllowAnonymous]
	public async Task<IActionResult> Login([FromBody][Required] LoginRequest request)
	{
		var found = await this._context.Users.AsNoTracking()
			.FirstOrDefaultAsync(x => x.Email == request.Email);

		if(found is null || !PasswordHelper.ConfirmPassword(request.Password, found.PasswordHash, found.PasswordSalt))
			return this.Unauthorized(InvalidCredentials);

		if(found.TotpSecretKeyForSha512 is not null)
		{
			if(request.TotpCode is null)
				return this.Continue(TotpRequired);

			if(!CheckTotp(found, request.TotpCode))
				return this.Unauthorized(TotpDidNotMatch);
		}

		return this.Ok(await IssueJwtAndAddToUser(found));
	}

	[HttpPut]
	[AllowAnonymous]
	public async Task<IActionResult> Register([FromBody][Required] RegistrationRequest request)
	{
		if(await this._context.Users.AnyAsync(x => x.Email.Equals(request.Email)))
			return await Login(new(request));

		// TODO: implement registration limit by nginx

		var passHash = PasswordHelper.HashPassword(request.Password, out var passSalt);
		var toAdd = new DbUser
		{
			Email = request.Email,
			PasswordSalt = passSalt,
			PasswordHash = passHash,
		};

		this._context.Users.Add(toAdd);
		await this._context.SaveChangesAsync();

		this._logger.LogTrace($"Added new user. " +
			$"Email: \'{toAdd.Email}\'. " +
			$"PassHashB64: \'{Convert.ToBase64String(toAdd.PasswordHash)}\'. " +
			$"PassSaltB64: \'{Convert.ToBase64String(toAdd.PasswordSalt)}\'. ");

		return await this.Login(new LoginRequest(request));
	}

	[HttpPatch]
	[AllowAnonymous]
	public async Task<IActionResult> Refresh()
	{
		var tkn = this.Request.Cookies[JwtRefreshTokenCookieName];
		if(tkn is null) return Unauthorized();

		var claims = _jwtService.ValidateRefreshJwt(tkn);
		if(claims is null) return Unauthorized();

		var foundUser = await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Guid == claims.Value.userGuid);
		if(foundUser is null) return Unauthorized();

		var foundToken = await _context.RefreshJtis.AsTracking().FirstOrDefaultAsync(x => x.JtiSha512 == SHA512.HashData(claims.Value.Jti));
		if(foundToken is null) return Unauthorized();

		if(foundToken.RelatedUserGuid != foundUser.Guid) return Unauthorized();

		var newToken = this.IssueJwtAndAddToUser(foundUser, true, foundToken);

		_context.RefreshJtis.Remove(foundToken);

		await this._context.SaveChangesAsync();
		return this.Ok(newToken);
	}

	[HttpDelete]
	public async Task<IActionResult> TerminateSession([FromQuery][Required] string jtiShaHex, [FromServices] MinimalJwtIatStorage sts)
	{
		var userGuid = this.ParseGuidClaim();
		var jtiHash = Convert.FromHexString(jtiShaHex);

		var found = await _context.RefreshJtis.FirstOrDefaultAsync(x => x.RelatedUserGuid == userGuid && x.JtiSha512.SequenceEqual(jtiHash));

		if(found is null) return NotFound();

		_context.RefreshJtis.Remove(found);
		await this._context.SaveChangesAsync();

		sts.SetUserMinimalJwtIat(found.Guid, DateTime.UtcNow.AddSeconds(-1));
		return this.Ok();
	}

	[HttpDelete]
	public async Task<IActionResult> TerminateSession()
	{
		var tkn = this.Request.Cookies[JwtRefreshTokenCookieName];
		if(tkn is null) return Unauthorized();

		var claims = _jwtService.ValidateRefreshJwt(tkn);
		if(claims is null) return Unauthorized();

		var jti = claims.Value.Jti;

		await _context.RefreshJtis.Where(x => x.JtiSha512.SequenceEqual(jti)).ExecuteDeleteAsync();

		return this.Ok();
	}

	[HttpPatch]
	[Authorize]
	[Route("password")]
	public async Task<IActionResult> ChangePassword([FromBody][Required] ChangePasswordRequest request)
	{
		var user = await this._context.Users.FirstOrDefaultAsync(x => x.Guid == this.ParseGuidClaim());
		if(user is null) return Unauthorized();

		SetPassword(user, request);

		await this._context.SaveChangesAsync();
		return this.Ok();
	}

	#region recovery
	private const string isRecoveryJwtClaim = nameof(isRecoveryJwtClaim);
	private const string emailJwtClaim = nameof(emailJwtClaim);
	private const string entropyClaim = nameof(entropyClaim);

	[HttpPut]
	[AllowAnonymous]
	[Route("recovery/{email}")]
	public async Task<IActionResult> CreateAndSendLink(
		[FromServices] EmailSendingService sender,
		[FromRoute][Required][EmailAddress] string email)
	{
		var found = await this._context.Users.FirstOrDefaultAsync(x => x.Email == email);
		if(found is null) return this.NotFound();

		var jti = RandomNumberGenerator.GetBytes(512 / 8);
		var jwt = _jwtService.GenerateRecoveryJwtToken(jti, found.Guid, TimeSpan.FromSeconds(_settings.RecoveryTokenLifespanSeconds));

		var now = DateTime.UtcNow;
		var geoInfo = GeoIpHeadersHelper.GetInfoFromRequest(this.Request);
		_context.RecoveryJtis.Add(new()
		{
			JtiSha512 = SHA512.HashData(jti),
			City = geoInfo.City,
			Country = geoInfo.Country,
			IPAddress = geoInfo.IpAddress,
			IssuedAt = now,
			OriginIssuedAt = now,
			RelatedUserGuid = found.Guid,
		});

		await this._context.SaveChangesAsync();

		if(!await sender.Send(new()
		{
			From = _settings.RecoveryEmailSendFromAddress,
			To = found.Email,
			Subject = "Password reset",
			FromName = "Web Testing Tool",
			Body = $"Your password reset link: {_settings.RecoveryEndpointWithoutToken}{jwt}"
		})) return Problem();
		return this.Ok();
	}

	[HttpPost]
	[AllowAnonymous]
	[Route("recovery/{jwt}")]
	public async Task<IActionResult> ResetPassword([FromRoute][Required] string jwt, [FromBody][Required] ChangePasswordRequest request)
	{
		_logger.LogInformation("Begin password reset.");

		if(string.IsNullOrEmpty(jwt)) // does this fuck even give us what we need
			return this.BadRequest();

		var parsed = _jwtService.ValidateRefreshJwt(jwt);
		if(parsed is null) return Unauthorized();

		var found = await _context.RecoveryJtis.FirstOrDefaultAsync(x => x.JtiSha512.SequenceEqual(parsed.Value.Jti));
		if(found is null) return Unauthorized();

		var user = await _context.Users.FirstOrDefaultAsync(x => x.Guid == found.RelatedUserGuid);
		if(user is null) return Unauthorized();

		SetPassword(user, request);
		await _context.SaveChangesAsync();

		return Ok();
	}
	#endregion


#if DEBUG
	[HttpDelete]
	[JwtAnonymous]
	[Route("all")]
	public async Task<IActionResult> DeleteAllUsers()
	{
		await _context.Users.Where(x=> true).ExecuteDeleteAsync();
		return Ok();
	}

#endif
}