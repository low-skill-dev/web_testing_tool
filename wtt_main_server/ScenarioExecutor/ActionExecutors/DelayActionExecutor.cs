using Models.Application.TestScenarios.ActionResults;
using Models.Database.TestScenarios;
using ScenarioExecutor.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.NetworkInformation;
using System.Reflection.PortableExecutable;
using Models.Application.TestScenarios.ActionResults;
using Models.Database.TestScenarios;
using ScenarioExecutor.Interfaces;
using static ScenarioExecutor.Helpers.ContextHelper;

namespace ScenarioExecutor.ActionExecutors;

public sealed class DelayActionExecutor : AActionExecutor<DbDelayAction, DelayActionResult>
{
	public DelayActionExecutor(DbDelayAction action) : base(action) { }

	public override async Task<Dictionary<string, string>> Execute(IDictionary<string, string> currentContext)
	{
		base.Start();

		if(Action.DelayMs > 0)
		{
			_cpuTimeCounter.Stop();
			await Task.Delay(Action.DelayMs);
			_cpuTimeCounter.Start();
		}

		await ExecuteUserScript(currentContext);
		var ret = (this.Result!.ContextUpdates as IEnumerable<(string n, string v)>).Reverse().DistinctBy(x => x.n).ToDictionary(x => x.n, x => x.v);

		base.Complete();
		return ret;
	}
}