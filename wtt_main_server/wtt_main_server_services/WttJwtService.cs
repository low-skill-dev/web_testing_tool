using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.ConstrainedExecution;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using webooster.Services;
using wtt_main_server_data.Api;
using wtt_main_server_data.Database.Common;

namespace wtt_main_server_services;

public class WttJwtService : JwtService
{
	public WttJwtService(ECDsa cert, ILogger<WttJwtService>? logger) : base(cert, logger)
	{

	}

	public string GenerateAccessJwt(DbUser user, TimeSpan lifespan)
		=> GenerateAccessJwt(new DbUserPublicInfo(user), lifespan);
	public string GenerateAccessJwt(DbUserPublicInfo user, TimeSpan lifespan)
	{
		var claims = new Claim[]
		{
			new(nameof(user.Guid),user.Guid.ToString(),ClaimValueTypes.String),
			new(nameof(user.Email), user.Email.ToString(), ClaimValueTypes.Email),
			new(nameof(user.Role), ((int)user.Role).ToString(), ClaimValueTypes.Integer32),
			new(nameof(user.IsDisabled), user.IsDisabled.ToString(), ClaimValueTypes.Boolean),
			new(nameof(user.IsEmailConfirmed), user.IsEmailConfirmed.ToString(), ClaimValueTypes.Boolean),
			new(nameof(user.RegistrationDate), user.RegistrationDate.ToString("O"), ClaimValueTypes.DateTime),
			new(nameof(user.PasswordLastChanged), user.PasswordLastChanged.ToString("O"), ClaimValueTypes.DateTime),
		};

		_logger?.LogTrace($"Created public info for user '{user.Guid}': {ClaimsToStr(claims)}.");

		return base.GenerateJwt(claims, lifespan);
	}

	public string GenerateRecoveryJwtToken(byte[] jti, Guid userGuid, TimeSpan lifespan)
		=> GenerateRefreshJwt(jti, userGuid, lifespan);
	public string GenerateRefreshJwt(byte[] jti, Guid userGuid, TimeSpan lifespan)
	{
		var encoded = Convert.ToHexString(jti);

		var claims = new Claim[]
		{
			new(JwtRegisteredClaimNames.Jti, encoded),
			new(JwtRegisteredClaimNames.Sub, userGuid.ToString())
		};

		_logger?.LogTrace($"Created refresh token claims for user '{userGuid}': {ClaimsToStr(claims)}.");

		return base.GenerateJwt(claims, lifespan);
	}

	public (byte[] Jti, Guid userGuid)? ValidateRecoveryJwtToken(string refreshJwt)
		=> ValidateRefreshJwt(refreshJwt);
	public (byte[] Jti, Guid userGuid)? ValidateRefreshJwt(string refreshJwt)
	{
		try
		{
			var claims = base.ValidateJwtToken(refreshJwt)!;

			var jti = Convert.FromHexString(claims.FindFirstValue(JwtRegisteredClaimNames.Jti)!);
			var uGuid = Guid.Parse(claims.FindFirstValue(JwtRegisteredClaimNames.Sub)!);

			_logger?.LogTrace($"Parsed refresh token '{refreshJwt}'.");
			return (jti, uGuid);
		}
		catch
		{
			_logger?.LogTrace($"Failed parsing refresh token '{refreshJwt}'.");
			return null;
		}
	}
}
