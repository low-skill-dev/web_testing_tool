using Microsoft.IdentityModel.Tokens;
using Models.Api;
using WebApi.Infrastructure.Authorization;
using static WebApi.Infrastructure.Authorization.JwtParseMiddleware;

namespace WebApi.Extensions;

public static class HttpContextExtensions
{
	public static bool IsJwtValid(this HttpContext ctx)
	{
		return ctx.Items.TryGetValue(OkFlagItemId, out _);
	}

	public static string? GetJwt(this HttpContext ctx)
	{
		return ctx.Items.TryGetValue(TokenItemId, out var tkn)
			? tkn as string : default;
	}

	public static DbUserJwtInfo? GetAuthedUser(this HttpContext ctx)
	{
		return ctx.Items.TryGetValue(UserItemId, out var usr)
			? usr as DbUserJwtInfo : default;
	}

	public static TokenValidationResult? GetJwtValidationResult(this HttpContext ctx)
	{
		return ctx.Items.TryGetValue(ResultItemId, out var rst)
		 ? rst as TokenValidationResult : default;
	}
}