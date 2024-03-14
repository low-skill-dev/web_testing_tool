using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using WebApi.Database;
using Models.Database.TestScenarios;
using Models.ServicesSettings;
using WebApi.Infrastructure.Authorization;
using WebApi.Extensions;
using static Models.Enums.UserRoles;
using Models.Enums;

namespace WebApi.Controllers.v1;

[ApiController]
[Route("api/v1/[controller]")]
[Consumes("application/json")]
[Produces("application/json")]
public class ScenarioController : ControllerBase
{
	private readonly WttContext _context;
	private readonly JwtServiceSettings _settings;
	private readonly ILogger<ScenarioController> _logger;

	public ScenarioController(WttContext context, JwtServiceSettings settings, ILogger<ScenarioController> logger)
	{
		_context = context;
		_settings = settings;
		_logger = logger;
	}

	[HttpGet]
	[JwtAuthorize]
	public async Task<IActionResult> GetScenarios([FromQuery] Guid? guid, [FromQuery] Guid? owner)
	{
		var user = this.HttpContext.GetAuthedUser()!;
		if(owner is not null && user.Role < Models.Enums.UserRoles.Moderator) return Unauthorized();

		var baseQuery = _context.TestScenarios.AsNoTracking();

		if(owner is not null)
		{
			if(user.Role < Moderator) return Unauthorized();
			else baseQuery = baseQuery.Where(x => x.UserGuid == owner);
		}
		else
		{
			baseQuery = baseQuery.Where(x => x.UserGuid == user.Guid);
		}

		if(guid is not null)
		{
			baseQuery = baseQuery.Where(x=> x.Guid == guid);
		}

		return Ok(await baseQuery.ToListAsync());
	}

	/* https://datatracker.ietf.org/doc/html/rfc5789
	 * PATCH Method for HTTP
	 *	Several applications extending the Hypertext Transfer Protocol (HTTP)
	 *	require a feature to do partial resource modification.  The existing
	 *	HTTP PUT method only allows a complete replacement of a document.
	 *	This proposal adds a new HTTP method, PATCH, to modify an existing
	 *  
	 *  ... So we do replacement here.
	 */
	[HttpPut]
	[JwtAuthorize]
	public async Task<IActionResult> WriteScenario([FromQuery][Required] string scenarioGuid, [FromBody][Required] DbTestScenario data)
	{
		var user = this.HttpContext.GetAuthedUser()!;
		var parsed = Guid.Parse(scenarioGuid);

		var found = await _context.TestScenarios.FirstOrDefaultAsync(x => x.Guid.Equals(parsed));

		if(found is not null)
		{
			if(!found.UserGuid.Equals(user) && user.Role < Administrator)
				//return Forbid("You are not the owner of this scenario.");
				return NotFound();

			var oldId = found.Guid;
			_context.Entry(found).CurrentValues.SetValues(data);
			found.Guid = oldId;
			found.Changed = DateTime.UtcNow;
		}
		else
		{
			data.Guid = Guid.NewGuid();
			_context.TestScenarios.Add(data);
		}

		await _context.SaveChangesAsync();
		return Ok(data.Guid);
	}
}
