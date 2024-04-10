using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Application.Abstract;
using Models.Application.TestScenarios.ActionResults;
using Models.Application.TestScenarios.Parameter;
using Models.Database.Abstract;
using Models.Database.Common;
using Models.Database.TestScenarios;
using ScenarioExecutor.Interfaces;
using static System.Collections.Specialized.BitVector32;

namespace ScenarioExecutor.ActionExecutors;

public sealed class EchoActionExecutor : AActionExecutor<DbEchoAction, EchoActionResult>
{
	public EchoActionExecutor(DbEchoAction action) : base(action) { }
	public override async  Task Execute(IDictionary<string, string> currentContext)
	{
		_cpuTimeCounter.Start();

		Result = new()
		{
			Started = DateTime.UtcNow
		};

		var (req, res) = await MakeRequestAsync(currentContext);

		await ExecuteUserScripts(currentContext, req, res);

		var ret = (this.Result!.ContextUpdates as IEnumerable<(string n, string v)>).Reverse().DistinctBy(x => x.n).ToDictionary(x => x.n, x => x.v);

		_cpuTimeCounter.Stop();
		Result.Completed = DateTime.UtcNow;
		return ret;
	}
}