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
public class DbScenarioAction : ADbAction
{
	public override ActionTypes Type { get; set; } = ActionTypes.DbTestScenarioType;

	// GUID сценария, который будет вызван
	public Guid CalledScenarioGuid { get; set; }

	//[Obsolete("Отказ от разработки по причине нехватки времени.")]
	//public Dictionary<string, string> Arguments { get; set; } = new();// name to value

	// Все переменные контекста исполненнОГО сценнария записать в переменную Х в текущем контексте.
	public string? WriteWriteOutputContextToVariable { get; set; }

	public bool? UseParentContextAsInitial { get; set; }
}
