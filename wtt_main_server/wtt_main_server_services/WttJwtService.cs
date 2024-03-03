using JwtService;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;
using System.Security.Cryptography;
using wtt_main_server_data.Api;
using wtt_main_server_data.Database.Common;
using wtt_main_server_data.Enums;
using wtt_main_server_data.ServicesSettings;

namespace wtt_main_server_services;

public sealed class WttJwtService
{
	private readonly IJwtService _service;
	private readonly WttJwtServiceSettings _settings;
	private readonly ILogger<WttJwtService>? _logger;

	public WttJwtService(
		IJwtService service,
		WttJwtServiceSettings settings,
		ILogger<WttJwtService>? logger)
	{
		_settings = settings;
		_service = service;
		_logger = logger;
	}

	public string GenerateAccessJwt(DbUser user, TimeSpan? lifespan = null)
	{
		var claims = new Claim[]
		{
			new(JwtRegisteredClaimNames.Sub, user.Guid.ToString(), ClaimValueTypes.String),
			new(nameof(DbUser.Email), user.Email.ToString(), ClaimValueTypes.Email),
			new(nameof(DbUser.Role), ((int)user.Role).ToString(), ClaimValueTypes.Integer32),
			new(nameof(DbUser.IsDisabled), user.IsDisabled.ToString(), ClaimValueTypes.Boolean),
			new(nameof(DbUser.RegistrationDate), user.RegistrationDate.ToString("O"), ClaimValueTypes.DateTime),
			new(nameof(DbUser.PasswordLastChanged), user.PasswordLastChanged.ToString("O"), ClaimValueTypes.DateTime),
			new(nameof(DbUser.EmailConfirmedAtUtc), user.EmailConfirmedAtUtc.ToString("O"), ClaimValueTypes.DateTime),
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


	public DbUserJwtInfo? ValidateAccessJwt(string token)
	{
		var result = _service.ValidateToken(token);

		return !result.IsValid ? null : new DbUserJwtInfo
		{
			Guid = Guid.Parse((string)result.Claims[JwtRegisteredClaimNames.Sub]),
			Email = (string)result.Claims[nameof(DbUser.Email)],
			Role = (UserRoles)((int)result.Claims[nameof(DbUser.Role)]),
			IsDisabled = (bool)result.Claims[nameof(DbUser.IsDisabled)],
			RegistrationDate = (DateTime)result.Claims[nameof(DbUser.RegistrationDate)],
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