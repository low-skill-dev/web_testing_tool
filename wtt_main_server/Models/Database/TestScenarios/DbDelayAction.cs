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
public class DbDelayAction : ADbAction
{
	public override ActionTypes Type { get; set; } = ActionTypes.DbDelayActionType;

	public int DelaySeconds { get; set; } = 0;
}
