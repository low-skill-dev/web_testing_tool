using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Application.Abstract;
using Models.Application.TestScenarios.ActionResults;
using Models.Application.TestScenarios.ScenarioRun;
using Models.Database.Abstract;
using Models.Database.Common;
using Models.Database.TestScenarios;
using ScenarioExecutor.Interfaces;

namespace ScenarioExecutor.ActionExecutors;

public sealed class ErrorActionExecutor : AActionExecutor<DbLogAction, ErrorActionResult>
{
	public ErrorActionExecutor(DbLogAction action) : base(action) { }

	public override async Task<Dictionary<string, string>> Execute(IDictionary<string, string> currentContext)
	{
		_cpuTimeCounter.Start();

		Result = new()
		{
			Started = DateTime.UtcNow,
			IsError = true,
		};

		await ExecuteUserScripts(currentContext);

		var ret = (this.Result!.ContextUpdates as IEnumerable<(string n, string v)>).Reverse().DistinctBy(x => x.n).ToDictionary(x => x.n, x => x.v);

		_cpuTimeCounter.Stop();
		Result.Completed = DateTime.UtcNow;
		return ret;
	}
}