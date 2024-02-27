using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wtt_main_server_data.Application.Abstract;
using wtt_main_server_data.Application.TestScenarios.ActionResults;
using wtt_main_server_data.Application.TestScenarios.Parameter;
using wtt_main_server_data.Database.Abstract;
using wtt_main_server_data.Database.Common;
using wtt_main_server_data.Database.TestScenarios;
using wtt_main_server_scenario_executor.Interfaces;
using static System.Collections.Specialized.BitVector32;

namespace wtt_main_server_scenario_executor.ActionExecutors;

public sealed class DelayActionExecutor : AActionExecutor<DbDelayAction, DelayActionResult>
{
	public DelayActionExecutor(DbDelayAction action) : base(action) { }

	public override async Task Execute(IDictionary<string, string> currentContext)
	{
		throw new NotImplementedException();
	}
}