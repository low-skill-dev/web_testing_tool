using Microsoft.AspNetCore.Mvc.Filters;

namespace wtt_main_server_api.Infrastructure;

public class JwtAnonymousAttribute : JwtAuthStrategyRequiredAttribute
{
	public override void OnAuthorization(AuthorizationFilterContext context)
	{

	}
}
