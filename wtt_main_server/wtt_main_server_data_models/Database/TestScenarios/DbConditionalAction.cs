using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wtt_main_server_data.Database.Abstract;

namespace wtt_main_server_data.Database.TestScenarios;

#pragma warning disable CS8618

public class DbConditionalAction : ADbAction
{
	public override string Type => "Conditional";


	public string JsBoolExpression { get; set; }

	// Replaced by ScenarioAction
	//public long? ScenarioIdCalledOnTrue { get; set; }
	//public long? ScenarioIdCalledOnFalse { get; set; }

	public Guid? ActionOnTrue { get; set; }
	public Guid? ActionOnFalse { get; set; }

	// Replaced by ErrorAction
	//public bool ThrowErrorOnTrue { get; set; } = false;
	//public bool ThrowErrorOnFalse { get;set; } = false;
}
