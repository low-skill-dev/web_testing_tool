using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Models.Database.RunningScenarios;
using ScenarioExecutor.ProjectInterface;
using WebApi.Database;

namespace WebApi.Services;

public class ScenarioExecutorBackgroundService : BackgroundService
{
	private readonly WttContext _ctx;
	private readonly IServiceProvider _ctxProvider;
	private readonly ScenarioSchedulerBackgroundService _scheduler;
	private readonly List<ScenarioExecutor.ProjectInterface.ScenarioExecutor> _inProgress;
	private readonly ILogger<ScenarioExecutorBackgroundService> _logger;

	private const int GlobalExecutionLimit = 128;
	 
	private const int loopDelaySeconds =
#if DEBUG
		1;
#else
		10;
#endif

	public ScenarioExecutorBackgroundService(WttContext ctx, IServiceProvider ctxProvider, ScenarioSchedulerBackgroundService scheduler, ILogger<ScenarioExecutorBackgroundService> logger)
	{
		_ctx = ctx;
		_scheduler = scheduler;
		_logger = logger;
		_inProgress = new();
		_ctxProvider = ctxProvider;
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		while(!stoppingToken.IsCancellationRequested)
		{
			await Task.Delay(TimeSpan.FromSeconds(loopDelaySeconds), stoppingToken);

			// can exceed actually
			await RemoveCompletedExecutions();
			while(_inProgress.Count > GlobalExecutionLimit)
			{
				await RemoveCompletedExecutions();
				await Task.Delay(TimeSpan.FromSeconds(loopDelaySeconds), stoppingToken);
			}

			var queue = _scheduler.GetQueue();
			var scenarios = await _ctx.TestScenarios.Where(x => queue.Contains(x.Guid)).Select(x => new { x.Guid, x.Name, x.EntryPoint, x.ActionsJson }).ToListAsync();
			foreach(var s in scenarios)
			{
				var ctxInstance = _ctxProvider.CreateScope().ServiceProvider.GetRequiredService<WttContext>();
				var execution = new ScenarioExecutor.ProjectInterface.ScenarioExecutor(new()
				{
					Guid = Guid.NewGuid(),
					DbScenarioGuid = s.Guid,
					EntryPoint = s.EntryPoint,
					ExecutionDepth = 1,
					ActionsLoadedFromDb = s.ActionsJson.ToDictionary(),
					LoadActionsByScenarioGuidFunc = async (g) =>
					{
						var s = await ctxInstance.TestScenarios.Where(x => x.Guid == g).FirstAsync();
						var actions = s.ActionsJson.ToDictionary();
						var entryPoint = s.EntryPoint;
						return (entryPoint, actions);
					},
					//DbExecutionLimitations = null,
				});

				_inProgress.Add(execution);

				_logger.LogInformation($"Scenario '{s.Guid.ToString().Substring(30, 6)}' was started.");
				_ = execution.StartAsync();
			}
		}
	}

	private async Task RemoveCompletedExecutions()
	{
		IEnumerable<ScenarioExecutor.ProjectInterface.ScenarioExecutor> completed;
		lock(_inProgress)
		{
			completed = _inProgress.Where(x => x.Progress.WasCompleted).ToList();
			_inProgress.RemoveAll(x => x.Progress.WasCompleted);
		}

		foreach(var s in completed)
			_logger.LogInformation($"Scenario '{s.Progress.RunInfo.DbScenarioGuid.ToString().Substring(30, 6)}' was completed.");

		await WriteResultsToDb(completed);
	}

	public bool IsExecuting(Guid g)
	{
		return _inProgress.Any(x => x.Progress.RunInfo.DbScenarioGuid == g);
	}

	private async Task WriteResultsToDb(IEnumerable<ScenarioExecutor.ProjectInterface.ScenarioExecutor> se)
	{
		var toWrite = se.Select(x => new DbScenarioRun
		{
			Guid = Guid.NewGuid(),
			ScenarioGuid = x.Progress.RunInfo.DbScenarioGuid,

			Started = x.Progress.Started,
			Completed = x.Progress.Completed,

			IsSucceeded = x.Progress.Exception is null,
			ErrorMessage = x.Progress.Exception?.Message,
		});

		_ctx.ScenarioRuns.AddRange(toWrite);
		await _ctx.SaveChangesAsync();
	}
}
