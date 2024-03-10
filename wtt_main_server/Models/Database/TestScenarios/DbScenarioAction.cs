using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Database.Abstract;

namespace Models.Database.TestScenarios;

#pragma warning disable CS8618

public class DbScenarioAction : ADbAction
{
	public override string Type => "Scenario";

	// GUID сценария, который будет вызван
	public Guid CalledScenarioGuid { get; set; }
	
	// Будет ли прервано исполнение текущего сценария в случае
	// ошибки в дочернем.
	public bool StopExecutionOnInternalError { get; set; }

	public Dictionary<string, string> Arguments { get; set; } // name to value

	public string? WriteAllResultToVariable { get; set; }

	// Имя переменной в результате к имени переменной в текущем контексте,
	// в которую будет записано значение
	public Dictionary<string, string> VariablesExtractedFromResult { get; set; }
}
