using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using webooster.Helpers;
using wtt_main_server_api.Database;
using wtt_main_server_api.Infrastructure;
using wtt_main_server_data.Api;
using wtt_main_server_data.Database.TestScenarios;
using wtt_main_server_data.ServicesSettings;
using wtt_main_server_services;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace wtt_main_server_api.Controllers.v1;

[JwtAuthorize]
[ApiController]
[Consumes("application/json")]
[Produces("application/json")]
[Route("api/v1/[controller]")]
public class ScenarioController : ControllerBase
{
	private readonly WttContext _context;
	private readonly AuthControllerSettings _settings;
	private readonly ILogger<ScenarioController> _logger;

	public ScenarioController(WttContext context, AuthControllerSettings settings, ILogger<ScenarioController> logger)
	{
		_context = context;
		_settings = settings;
		_logger = logger;
	}

	[HttpGet]
	public async Task<IActionResult> GetScenario([FromQuery][Required] string guid)
	{
		if(!Guid.TryParse(guid, out var scenGuid)) return BadRequest(nameof(guid));

		var userGuid = this.ParseGuidClaim();
		var found = await _context.TestScenarios.AsNoTracking()
			.FirstOrDefaultAsync(x => x.RelatedUserGuid.Equals(userGuid) && x.Guid.Equals(scenGuid));

		if(found is null) return NotFound(nameof(DbTestScenario));

		return Ok(found);
	}

	[HttpGet]
	[Route("my")]
	public async Task<IActionResult> GetMyScenarios()
	{
		var userGuid = this.ParseGuidClaim();
		var found = await _context.TestScenarios.AsNoTracking()
			.Where(x => x.RelatedUserGuid.Equals(userGuid)).ToListAsync();

		return Ok(found);
	}

	[HttpGet]
	[Route("my/hash")]
	public async Task<IActionResult> GetMyScenariosHashes()
	{
		var userGuid = this.ParseGuidClaim();
		var found = await _context.TestScenarios.AsNoTracking()
			.Where(x => x.RelatedUserGuid.Equals(userGuid))
			.Select(x => new { x.Guid, x.Sha512 })
			.ToListAsync();

		return Ok(found);
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
	public async Task<IActionResult> WriteScenario([FromQuery][Required] string scenarioGuid, [FromBody][Required] DbTestScenario data)
	{
		var user = this.ParseGuidClaim();
		var parsed = Guid.Parse(scenarioGuid);

		var found = await _context.TestScenarios.FirstOrDefaultAsync(x => x.Guid.Equals(parsed));

		if(found is not null)
		{
			if(!found.RelatedUserGuid.Equals(user))
				return Forbid("You are not the owner of this scenario.");

			var oldId = found.Id;
			_context.Entry(found).CurrentValues.SetValues(data);
			found.Id = oldId;
			found.ChangeDate = DateTime.UtcNow;
		}
		else
		{
			data.Id = 0;
			data.Guid = BinaryGuid.Create();
			_context.TestScenarios.Add(data);
		}

		await _context.SaveChangesAsync();

		return Ok(data.Guid);
	}
}
