using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using wtt_main_server_data.Database.Common;
using wtt_main_server_data.Enums;

namespace wtt_main_server_api.Controllers;

public static class ControllerExtensions
{

	/// <exception cref="ArgumentNullException"/>
	/// <exception cref="FormatException"/>
	/// <exception cref="OverflowException"/>
	[NonAction]
	public static Guid ParseGuidClaim(this ControllerBase ctr) => Guid.Parse(ctr.User.FindFirstValue(nameof(DbUser.Guid))!);

	/// <exception cref="ArgumentNullException"/>
	/// <exception cref="FormatException"/>
	/// <exception cref="OverflowException"/>
	[NonAction]
	public static UserRoles ParseRoleClaim(this ControllerBase ctr) => (UserRoles)int.Parse(ctr.User.FindFirstValue(nameof(DbUser.Role))!);

	/// <summary>
	/// Creates an Microsoft.AspNetCore.Mvc.ObjectResult that produces a 
	/// Microsoft.AspNetCore.Http.StatusCodes.Status100Continue response.
	/// </summary>
	public static ObjectResult Continue(this ControllerBase ctr, object? value) => ctr.StatusCode(StatusCodes.Status100Continue, value);
}
