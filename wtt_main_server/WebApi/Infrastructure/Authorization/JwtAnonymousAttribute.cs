using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApi.Infrastructure.Authorization;

public class JwtAnonymousAttribute : JwtAuthStrategyAttribute
{
    public override void OnAuthorization(AuthorizationFilterContext context)
    {

    }
}
