using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models.Database.RunningScenarios;
using Models.Database.Abstract;
using Models.Application.Abstract;
using Models.Enums;

namespace Models.Application.TestScenarios.ScenarioRun;
public sealed class ScenarioProgressInfo
{
	public ScenarioRunInfo RunInfo { get; init; }

	// Текущее состояние значений переменных.
	public Dictionary<string, string> CurrentVariableContext { get; set; } = new();

	public List<AActionResult> ActionResults { get; init; } = new();

	// Отражает общее число выполненных действий.
	public long ExecutionCount { get; set; } = 0;

	public bool WasStarted => Started.HasValue;
	public bool WasCompleted => Completed.HasValue;
	public Exception? Exception { get; set; } = null;

	public DateTime? Started { get; set; }
	public DateTime? Completed { get; set; }

	public ScenarioProgressInfo(ScenarioRunInfo runInfo)
	{
		this.RunInfo = runInfo;
	}
}