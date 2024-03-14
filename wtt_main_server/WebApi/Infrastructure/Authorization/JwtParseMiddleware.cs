using static Models.Constants.ClaimNames;
using static Models.Constants.CookieNames;
using static Models.Constants.CookiePaths;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using wtt_main_server_services;
using System.Runtime.ConstrainedExecution;
using Models.Database.Common;
using SharedServices;
using Models.Api;
using Microsoft.IdentityModel.Tokens;
using CommonLibrary.Services.Jwt;

namespace WebApi.Infrastructure.Authorization;

/// <summary>
/// Данное мидлваре извлекает из контекста поступившего запроса
/// JWT токен, хранящийся, по-умолчанию, в куках, имя которого
/// задаётся соответствующей константой <see cref="JwtAccessTokenCookieName"/>.
/// В случае успешного извлечения, все клаймы из токена добавляются
/// к <see cref="ClaimsPrincipal"/> <see cref="HttpContext.User"/>.
/// Далее предполагается использование как просто для извлечения данных,
/// так и для создания <see cref="IAuthorizationRequirement"/>.
/// </summary>
public sealed class JwtParseMiddleware : IMiddleware
{
	private const string jwt = "jwt";
	private const string underscore = "_";
	private const string prefix = jwt + underscore;

	private const string isOkItem = prefix + "isOk";
	private const string issItem = prefix + "iss";
	private const string iatItem = prefix + "iat";
	private const string expItem = prefix + "exp";
	private const string tknItem = prefix + "tkn";
	private const string usrItem = prefix + "usr";
	private const string rstItem = prefix + "rst";

	public const string OkFlagItemId = isOkItem;
	public const string ResultItemId = rstItem;
	public const string TokenItemId = tknItem;
	public const string UserItemId = usrItem;

	private readonly WttJwtService _jwtService;
	private readonly JwtMinIatStorage _minIatStorage;

	public JwtParseMiddleware(/*[FromServices]*/ WttJwtService jwtService, /*[FromServices]*/ JwtMinIatStorage minIatStorage)
	{
		_jwtService = jwtService;
		_minIatStorage = minIatStorage;
	}

	public async Task InvokeAsync(HttpContext context, RequestDelegate next)
	{
		bool ok = true;
		string? token = null;
		DbUserJwtInfo? parsed = null;
		TokenValidationResult? result = null;
		try
		{
			token = context.Request.Cookies[JwtAccessTokenCookieName];
			parsed = _jwtService.ValidateAccessJwt(token!, out result);
			ArgumentNullException.ThrowIfNull(token);
			ArgumentNullException.ThrowIfNull(parsed);
			ArgumentNullException.ThrowIfNull(result);

			var minIat = _minIatStorage.GetObject(parsed.Guid);
			if(minIat != default && result.GetIat() < minIat)
				throw new SecurityTokenExpiredException();
		}
		catch
		{
			ok = false;
		}

		if(ok && (result?.IsValid ?? false))
		{
			context.Items.Add(OkFlagItemId, true);
			context.Items.Add(UserItemId, parsed);
			context.Items.Add(TokenItemId, token);
			context.Items.Add(ResultItemId, result);
		}

		await next.Invoke(context);
	}
}
