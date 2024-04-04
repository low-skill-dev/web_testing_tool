using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Database.Abstract;
using Models.Enums;
using Reinforced.Typings.Attributes;

namespace Models.Database.TestScenarios;

#pragma warning disable CS8618

//[TsClass(IncludeNamespace = false, Order = 500)]
public class DbConditionalAction : ADbAction
{
	public override ActionTypes Type { get; set; } = ActionTypes.DbConditionalActionType;

	public string JsBoolExpression { get; set; } = "";

	// Replaced by ScenarioAction
	//public long? ScenarioIdCalledOnTrue { get; set; }
	//public long? ScenarioIdCalledOnFalse { get; set; }

	public Guid? ActionOnTrue { get; set; }
	public Guid? ActionOnFalse { get; set; }

	// Replaced by ErrorAction
	//public bool ThrowErrorOnTrue { get; set; } = false;
	//public bool ThrowErrorOnFalse { get;set; } = false;
}
