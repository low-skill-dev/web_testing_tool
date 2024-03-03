using static wtt_main_server_data.Constants.ClaimNames;
using static wtt_main_server_data.Constants.CookieNames;
using static wtt_main_server_data.Constants.CookiePaths;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using wtt_main_server_services;
using System.Runtime.ConstrainedExecution;
using wtt_main_server_data.Database.Common;

namespace wtt_main_server_api.Infrastructure;

/// <summary>
/// Данное мидлваре извлекает из контекста поступившего запроса
/// JWT токен, хранящийся, по-умолчанию, в куках, имя которого
/// задаётся соответствующей константой <see cref="JwtAccessTokenCookieName"/>.
/// В случае успешного извлечения, все клаймы из токена добавляются
/// к <see cref="ClaimsPrincipal"/> <see cref="HttpContext.User"/>.
/// Далее предполагается использование как просто для извлечения данных,
/// так и для создания <see cref="IAuthorizationRequirement"/>.
/// </summary>
public sealed class JwtTokenParseMiddleware : IMiddleware
{
	private const string jwt = "jwt";
	private const string underscore = "_";
	private const string prefix = jwt + underscore;

	public const string isOkClaim = prefix + "isOk";
	public const string isOkItem = prefix + "isOk";
	public const string issItem = prefix + "iss";
	public const string iatItem = prefix + "iat";
	public const string expItem = prefix + "exp";
	public const string tknItem = prefix + "tkn";

	public const string isOkFalse = "f";
	public const string isOkTrue = "t";

	private readonly JwtService _jwtService;
	private readonly string _jwtTokenCookieName;
	private readonly MinimalJwtIatStorage _minIatStorage;

	public JwtTokenParseMiddleware([FromServices] JwtService jwtService, [FromServices] MinimalJwtIatStorage minIatStorage, string jwtTokenCookieName)
	{
		this._jwtService = jwtService;
		this._minIatStorage = minIatStorage;
		this._jwtTokenCookieName = jwtTokenCookieName;
	}

	public Task InvokeAsync(HttpContext context, RequestDelegate next)
	{
		if(!context.Request.Cookies.TryGetValue(_jwtTokenCookieName, out var token))
		{
			return next.Invoke(context);
		}

		ClaimsPrincipal parsed;
		DateTime iatValue, expValue;
		string issuer;
		try
		{
			parsed = _jwtService.ValidateJwtToken(token, out iatValue, out expValue, out issuer)
				?? throw new AggregateException(nameof(_jwtService.ValidateJwtToken));

			if(expValue < DateTime.UtcNow)
				throw new AggregateException(nameof(expValue));
		}
		catch
		{
			return next.Invoke(context);
		}

		if(!_minIatStorage.ValidateIat(parsed.FindFirstValue(nameof(DbUser.Guid))!, iatValue))
		{
			return next.Invoke(context);
		}

		parsed.AddIdentity(isOkClaim, true.ToString());

		context.User = parsed;
		context.Items.Add(isOkItem, true);
		context.Items.Add(tknItem, token);
		context.Items.Add(issItem, issuer);
		context.Items.Add(iatItem, iatValue);
		context.Items.Add(expItem, expValue);

		return next.Invoke(context);
	}
}
