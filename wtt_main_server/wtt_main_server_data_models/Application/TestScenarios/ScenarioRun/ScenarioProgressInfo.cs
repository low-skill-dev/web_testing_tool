using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wtt_main_server_data.Application.TestScenarios.Parameter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using wtt_main_server_data.Database.RunningScenarios;
using wtt_main_server_data.Database.Abstract;
using webooster.Helpers;
using wtt_main_server_data.Application.Abstract;
using wtt_main_server_data.Enums;

namespace wtt_main_server_data.Application.TestScenarios.ScenarioRun;
public sealed class ScenarioProgressInfo
{
	public ScenarioRunInfo RunInfo { get; init; }

	/* Снапшот состояние контекста перменных для каждого действия.
	 * Служит для отладки. Опционально создаёт админом для
	 * какого-либо сценария. По дефолту остается пустым.
	 */
	public List<Dictionary<string, ScenarioArgument>> ContextByAction { get; init; } = new();

	// Текущее состояние значений переменных.
	public Dictionary<string, string> CurrentVariableContext { get; init; } = new();

	public List<AActionResult> ActionResults { get; init; } = new();

	// Отражает общее число выполненных действий.
	public long ExecutionCount { get; set; } = 0;

	public bool WasStarted => Started.HasValue;
	public bool WasCompleted => Completed.HasValue;
	public Exception? Exception { get; set; } = null;


	public DateTime? Started { get; set; }
	public DateTime? Completed { get; set; }
	public TimeSpan? ProcessorTime { get; set; }

	public ScenarioProgressInfo(ScenarioRunInfo runInfo)
	{
		this.RunInfo = runInfo;
	}
}