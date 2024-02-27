using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace wtt_main_server_api.Infrastructure;

// https://stackoverflow.com/a/68974923
public sealed class JwtAuthorizeAttribute : JwtAuthStrategyRequiredAttribute
{
	public override void OnAuthorization(AuthorizationFilterContext context)
	{
		if(!(context.HttpContext.User.FindFirstValue(JwtTokenParseMiddleware.isOkClaim)?.Equals(JwtTokenParseMiddleware.isOkTrue) ?? false))
		{
			context.Result = new StatusCodeResult(StatusCodes.Status401Unauthorized);
		}
	}
}
