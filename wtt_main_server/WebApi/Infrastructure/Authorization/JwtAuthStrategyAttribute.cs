using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApi.Infrastructure.Authorization;

public abstract class JwtAuthStrategyAttribute : Attribute, IAuthorizationFilter
{
    public abstract void OnAuthorization(AuthorizationFilterContext context);
}
