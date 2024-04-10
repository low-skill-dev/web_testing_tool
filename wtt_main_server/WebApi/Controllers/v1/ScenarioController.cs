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
using Models.Database.RunningScenarios;

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
			baseQuery = baseQuery.Where(x => x.Guid == guid);
		}

		return Ok(await baseQuery.ToListAsync());
	}

	///* https://datatracker.ietf.org/doc/html/rfc5789
	// * PATCH Method for HTTP
	// *	Several applications extending the Hypertext Transfer Protocol (HTTP)
	// *	require a feature to do partial resource modification.  The existing
	// *	HTTP PUT method only allows a complete replacement of a document.
	// *	This proposal adds a new HTTP method, PATCH, to modify an existing
	// *  
	// *  ... So we do replacement here.
	// */
	//[HttpPut]
	//[JwtAuthorize]
	//public async Task<IActionResult> WriteScenario([FromBody][Required] DbTestScenario data)
	//{
	//	var user = this.HttpContext.GetAuthedUser()!;

	//	var found = await _context.TestScenarios.FirstOrDefaultAsync(x => x.Guid.Equals(data.Guid));

	//	if(found is not null)
	//	{
	//		if(!found.UserGuid.Equals(user) && user.Role < Administrator)
	//			//return Forbid("You are not the owner of this scenario.");
	//			return NotFound();

	//		var oldId = found.Guid;
	//		_context.Entry(found).CurrentValues.SetValues(data);
	//		found.Guid = oldId;
	//		found.Changed = DateTime.UtcNow;
	//	}
	//	else
	//	{
	//		data.Guid = Guid.NewGuid();
	//		_context.TestScenarios.Add(data);
	//	}

	//	await _context.SaveChangesAsync();
	//	return Ok(data.Guid);
	//}

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
	public async Task<IActionResult> WriteScenarios([FromBody][Required] DbTestScenario[] data)
	{
		var user = this.HttpContext.GetAuthedUser()!;

		var guids = data.Select(x => x.Guid).ToList();
		var found = await _context.TestScenarios.Where(x => guids.Contains(x.Guid)).Select(x => new { x.Guid, x.UserGuid }).ToListAsync();

		if(user.Role < Administrator && found.Any(x => x.UserGuid != user.Guid))
			return Forbid();

		var foundGuids = found.Select(x => x.Guid).ToHashSet();

		await _context.Database.BeginTransactionAsync(System.Data.IsolationLevel.ReadUncommitted);

		try
		{
			foreach(var scenarioFromClient in data)
			{

				if(foundGuids.Contains(scenarioFromClient.Guid))
				{
					await _context.TestScenarios.Where(x => x.Guid == scenarioFromClient.Guid).ExecuteUpdateAsync(s =>
						s.SetProperty(x => x.ActionsJson, scenarioFromClient.ActionsJson)
						.SetProperty(x => x.Name, scenarioFromClient.Name)
						.SetProperty(x => x.ArgNames, scenarioFromClient.ArgNames)
						.SetProperty(x => x.ArgTypes, scenarioFromClient.ArgTypes)
						.SetProperty(x => x.EnableEmailNotifications, scenarioFromClient.EnableEmailNotifications)
						.SetProperty(x => x.EntryPoint, scenarioFromClient.EntryPoint)
						.SetProperty(x => x.RunIntervalMinutes, scenarioFromClient.RunIntervalMinutes)
						);
				}
				else
				{
					scenarioFromClient.Guid = Guid.NewGuid();
					scenarioFromClient.UserGuid = user.Guid;
					_context.TestScenarios.Add(scenarioFromClient);
				}
			}

			var newGuids = data.Select(x => x.Guid).ToList();
			await _context.TestScenarios.Where(x => x.UserGuid == user.Guid && !newGuids.Contains(x.Guid)).ExecuteDeleteAsync();
			var i = await _context.SaveChangesAsync();
		}
		catch(Exception ex)
		{
			await _context.Database.RollbackTransactionAsync();
			return Problem();
		}

		await _context.Database.CommitTransactionAsync();

		return Ok();
	}

	[HttpGet]
	[Route("logs")]
	[JwtAuthorize]
	public async Task<IActionResult> GetLogs()
	{
		var user = this.HttpContext.GetAuthedUser()!;
		var userScenarios = await _context.TestScenarios.Where(x => x.UserGuid == user.Guid).Select(x => x.Guid).ToListAsync();
		var userLogs = await _context.ScenarioRuns.Where(x => userScenarios.Contains(x.ScenarioGuid)).ToListAsync();

		var ret = userLogs.GroupBy(x => x.ScenarioGuid).Select(x => new ScenarioGuidToRuns { g = x.Key, r = x.OrderByDescending(x=> x.Completed).ToArray() }).ToList();

		return Ok(ret);
	}
	class ScenarioGuidToRuns
	{
		public Guid g { get; set; }
		public DbScenarioRun[] r { get; set; }
	}

}