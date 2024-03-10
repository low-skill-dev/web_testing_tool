using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Database.RunningScenarios;
using Models.Database.Abstract;
using Models.Application.TestScenarios.Parameter;
using Models.Database.Common;

//using wtt_main_server_database_models.Database;

namespace Models.Application.TestScenarios.ScenarioRun;
public sealed class ScenarioRunInfo
{
	// GUID текущего запуска сценария
	public Guid Guid { get; init; } = Guid.NewGuid();

	// GUID сценария в базе данных
	public required Guid DbScenarioGuid { get; init; }

	// GUID сценария, который вызвал данный сценарий
	public required Guid? Parent { get; init; }

	// Уровень вложенности в текущем сценарии.
	// Альтернативно ручному заданию значения, можно вычислить
	// путем подъема вверх к наивысшему родителю, считая
	// число шагов, но YAGNI подсказывает, что не стоит...
	public required long ExecutionDepth { get; init; }


	/// <summary>
	/// GUID действия, которое является точкой входа.
	/// </summary>
	public required Guid EntryPoint { get; init; }

	/// <summary>
	/// Действия, загруженные из базы данных для текущего сценария.
	/// </summary>
	public required Dictionary<Guid, ADbAction> ActionsLoadedFromDb { get; init; }

	// Уровень доступа пользователя к ресурсам
	public required DbTariff? DbExecutionLimitations { get; init; }

	/// <summary>
	/// Параметры, переданные в текущий сценарий извне.
	/// </summary>
	public required Dictionary<string, string> Arguments { get; init; }
}
