using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;
using System.Security.Cryptography;
using Models.Api;
using Models.Database.Common;
using Models.Enums;
using Models.ServicesSettings;
using static CommonLibrary.Services.Jwt.Extensions;
using CommonLibrary.Services.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace wtt_main_server_services;

public sealed class WttJwtService
{
	private readonly IJwtService _service;
	private readonly JwtServiceSettings _settings;
	private readonly ILogger<WttJwtService>? _logger;

	public WttJwtService(
		IJwtService service,
		JwtServiceSettings settings,
		ILogger<WttJwtService>? logger)
	{
		_settings = settings;
		_service = service;
		_logger = logger;
	}

	public string GenerateAccessJwt(DbUser user, TimeSpan? lifespan = null)
	{
		var info = new DbUserPublicInfo(user);
		var claims = new Claim[]
		{
			new(JwtRegisteredClaimNames.Sub, info.Guid.ToString(), ClaimValueTypes.String),
			new(nameof(DbUser.Email), info.Email.ToString(), ClaimValueTypes.Email),
			new(nameof(DbUser.Role), ((int)info.Role).ToString(), ClaimValueTypes.Integer32),
			new(nameof(DbUser.IsDisabled), info.IsDisabled.ToString(), ClaimValueTypes.Boolean),
			new(nameof(DbUser.Created), info.Created.ToString("O"), ClaimValueTypes.DateTime),
			new(nameof(DbUser.PasswordLastChanged), info.PasswordLastChanged.ToString("O"), ClaimValueTypes.DateTime),
			new(nameof(DbUser.EmailConfirmedAtUtc), info.EmailConfirmedAtUtc?.ToString("O") ?? null!, ClaimValueTypes.DateTime),
		};

		_logger?.LogInformation($"Creating access token: {claims.ToStringRepresentation()}.");

		return _service.CreateToken(claims, lifespan ?? TimeSpan.FromSeconds(_settings.AccessTokenLifespanSeconds));
	}

	public string GenerateRefreshJwt(DbUser user, out byte[] jti, TimeSpan? lifespan = null)
	{
		jti = RandomNumberGenerator.GetBytes(512 / 8);

		var claims = new Claim[]
		{
			new(JwtRegisteredClaimNames.Sub, user.Guid.ToString(), ClaimValueTypes.String),
			new(JwtRegisteredClaimNames.Jti, Convert.ToHexString(jti), ClaimValueTypes.String),
			new(nameof(user.Email), user.Email.ToString(), ClaimValueTypes.Email),
		};

		_logger?.LogInformation($"Creating refresh token: {claims.ToStringRepresentation()}.");

		return _service.CreateToken(claims, lifespan ?? TimeSpan.FromSeconds(_settings.RefreshTokenLifespanSeconds));
	}

	public string GenerateRecoveryJwt(DbUser user, out byte[] jti, TimeSpan? lifespan = null)
	{
		return GenerateRefreshJwt(user, out jti, lifespan ?? TimeSpan.FromSeconds(_settings.RecoveryTokenLifespanSeconds));
	}


	public DbUserJwtInfo? ValidateAccessJwt(string token, out TokenValidationResult? result)
	{
		result = _service.ValidateToken(token);

		return !result.IsValid ? null : new DbUserJwtInfo
		{
			Guid = Guid.Parse((string)result.Claims[JwtRegisteredClaimNames.Sub]),
			Email = (string)result.Claims[nameof(DbUser.Email)],
			Role = (UserRoles)((int)result.Claims[nameof(DbUser.Role)]),
			IsDisabled = (bool)result.Claims[nameof(DbUser.IsDisabled)],
			Created = (DateTime)result.Claims[nameof(DbUser.Created)],
			PasswordLastChanged = (DateTime)result.Claims[nameof(DbUser.PasswordLastChanged)],
			EmailConfirmedAtUtc = (DateTime)result.Claims[nameof(DbUser.EmailConfirmedAtUtc)],
		};
	}

	public RefreshJwtInfo? ValidateRefreshJwt(string token)
	{
		var result = _service.ValidateToken(token);

		return !result.IsValid ? null : new RefreshJwtInfo
		{
			UserGuid = Guid.Parse((string)result.Claims[JwtRegisteredClaimNames.Sub]),
			Jti = Convert.FromHexString((string)result.Claims[JwtRegisteredClaimNames.Jti])
		};
	}

	public RefreshJwtInfo? ValidateRecoveryJwt(string token)
	{
		return ValidateRefreshJwt(token);
	}
}