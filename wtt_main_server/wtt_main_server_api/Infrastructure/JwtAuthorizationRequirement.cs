//using Microsoft.AspNetCore.Authorization;
//using System.Security.Claims;

//namespace wtt_main_server_api.Infrastructure;

//public sealed class JwtAuthorizationRequirement : AuthorizationHandler<JwtAuthorizationRequirement>, IAuthorizationRequirement
//{
//	protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, JwtAuthorizationRequirement requirement)
//	{
//		const string failureReason = "jwt_error";

//		if(context.User.FindFirstValue(JwtTokenParseMiddleware.isOkClaim)?.Equals(JwtTokenParseMiddleware.isOkTrue) ?? false)
//			context.Succeed(requirement);
//		else
//			context.Fail(new(this, failureReason));

//		return Task.CompletedTask;
//	}
//}
