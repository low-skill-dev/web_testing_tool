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
	public IDictionary<string, string> CurrentVariableContext { get; set; }

	public List<AActionResult> ActionResults { get; init; } = new();

	// Отражает общее число выполненных действий.
	public long ExecutionCount { get; set; } = 0;

	public bool WasCompleted => Completed.HasValue;
	public Exception? Exception { get; set; } = null;

	public DateTime? Started { get; set; }
	public DateTime? Completed { get; set; }

	public long ProcessorTicksCount { get; set; }

	public ScenarioProgressInfo(ScenarioRunInfo runInfo)
	{
		this.RunInfo = runInfo;

		if(runInfo.InitialContext is not null)
			this.CurrentVariableContext = runInfo.InitialContext
			.ToDictionary(x => string.Copy(x.Key), x => string.Copy(x.Value));
		else
			this.CurrentVariableContext = new Dictionary<string, string>();
	}
}