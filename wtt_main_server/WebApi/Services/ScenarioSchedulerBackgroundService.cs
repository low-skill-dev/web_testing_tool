
using Microsoft.EntityFrameworkCore;
using WebApi.Database;

namespace WebApi.Services;

public class ScenarioSchedulerBackgroundService : BackgroundService
{
	private readonly WttContext _ctx;
	private readonly HashSet<Guid> _waitingToRun;

	public ScenarioSchedulerBackgroundService(WttContext ctx)
	{
		_ctx = ctx;
		_waitingToRun = new();
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		//await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);

		while(!stoppingToken.IsCancellationRequested)
		{
#if DEBUG
			await Task.Delay(TimeSpan.FromMinutes(0.1), stoppingToken);
#else
			await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
#endif
			var scenarios = await _ctx.TestScenarios.Where(x => x.RunIntervalMinutes > 0).Select(x => new { x.Guid, x.RunIntervalMinutes }).ToListAsync();

			var runLogs = (await _ctx.ScenarioRuns.OrderByDescending(x => x.Created).ToListAsync()).DistinctBy(x => x.ScenarioGuid).ToList();

			var now = DateTime.UtcNow;
			var needToRun = scenarios
				.Where(s => (now - (runLogs.FirstOrDefault(x => x.ScenarioGuid == s.Guid)?.Created ?? DateTime.MinValue))
					.TotalMinutes > s.RunIntervalMinutes)
				.Select(x => x.Guid).Where(x => !_waitingToRun.Contains(x));

			foreach(var g in needToRun) _waitingToRun.Add(g);
		}
	}

	public bool IsScheduled(Guid g)
	{
		return _waitingToRun.Contains(g);
	}

	public bool Enqueue(Guid g)
	{
		return _waitingToRun.Add(g);
	}

	public List<Guid> GetQueue()
	{
		List<Guid> ret;
		lock(_waitingToRun)
		{
			ret = _waitingToRun.ToList();
			_waitingToRun.Clear();
		}
		return ret;
	}
}