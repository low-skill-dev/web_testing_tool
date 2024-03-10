using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebApi.Extensions;

namespace WebApi.Infrastructure.Authorization;

// https://stackoverflow.com/a/68974923
public sealed class JwtAuthorizeAttribute : JwtAuthStrategyAttribute
{
    public override void OnAuthorization(AuthorizationFilterContext context)
    {
        if (!context.HttpContext.IsJwtValid())
        {
            context.Result = new UnauthorizedObjectResult("JWT validation failed");
        }
    }
}
