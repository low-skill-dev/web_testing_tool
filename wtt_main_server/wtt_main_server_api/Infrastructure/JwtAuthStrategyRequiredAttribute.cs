using Microsoft.AspNetCore.Mvc.Filters;

namespace wtt_main_server_api.Infrastructure;

public class JwtAuthStrategyRequiredAttribute : Attribute, IAuthorizationFilter
{
	public virtual void OnAuthorization(AuthorizationFilterContext context)
	{
		throw new NotImplementedException("There was no JWT auth strategy specified.");
	}
}
