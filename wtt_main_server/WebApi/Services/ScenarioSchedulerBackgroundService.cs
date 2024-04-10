
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

			var runLogs = await _ctx.ScenarioRuns.OrderByDescending(x => x.Created)/*.DistinctBy(x => x.ScenarioGuid)*/.ToListAsync();

			var now = DateTime.UtcNow;
			var needToRun = scenarios

#if DEBUG

#else
				.Where(s => (now - runLogs.First(x => x.ScenarioGuid == s.Guid).Created).TotalMinutes > s.RunIntervalMinutes)
#endif
				.Select(x => x.Guid).Where(x=> !_waitingToRun.Contains(x));

			foreach(var g in needToRun) _waitingToRun.Add(g);
		}
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