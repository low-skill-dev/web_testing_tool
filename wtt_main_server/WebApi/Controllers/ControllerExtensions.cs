using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

public static class ControllerExtensions
{
	/// <summary>
	/// Creates an Microsoft.AspNetCore.Mvc.ObjectResult that produces a 
	/// Microsoft.AspNetCore.Http.StatusCodes.Status100Continue response.
	/// </summary>
	public static ObjectResult Continue(this ControllerBase ctr, object? value) => ctr.StatusCode(StatusCodes.Status100Continue, value);

	/// <summary>
	/// Creates an Microsoft.AspNetCore.Mvc.ObjectResult that produces a 
	/// Microsoft.AspNetCore.Http.StatusCodes.Status503ServiceUnavailable response.
	/// </summary>
	public static ObjectResult ServiceUnavailable(this ControllerBase ctr, object? value) => ctr.StatusCode(StatusCodes.Status503ServiceUnavailable, value);
}
