using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Models.Application.Abstract;
using Models.Application.TestScenarios.ActionResults;
using Models.Application.TestScenarios.ScenarioRun;
using Models.Database.Abstract;
using Models.Database.Common;
using Models.Database.TestScenarios;
using ScenarioExecutor.Interfaces;
using static System.Collections.Specialized.BitVector32;
using Jint;
using System.Text;
using Models.Application.Abstract;
using Models.Application.TestScenarios.ActionResults;
using Models.Database.Abstract;
using Models.Database.Common;
using Models.Database.TestScenarios;
using Models.Database.TestExecutors;
using ScenarioExecutor.Helpers;
using ScenarioExecutor.Interfaces;
using static ScenarioExecutor.Helpers.ContextHelper;
using System.Diagnostics;
using static System.Text.Json.JsonSerializer;
using System;
using Models.Constants;
using CommonLibrary.Helpers;
using Jint.Runtime;


namespace ScenarioExecutor.ActionExecutors;

/// <summary>
/// Действие для вызова другого сценария из текущего
/// </summary>
public sealed class ScenarioActionExecutor : AActionExecutor<DbScenarioAction, ScenarioActionResult>
{
	public Func<Guid, Task<(Guid EntryPoint, Dictionary<Guid, ADbAction> Actions)>> LoadActionsByScenarioGuidFunc { get; set; }
	public int CallerExecutionDepth { get; set; }
	public Guid CallerGuid { get; set; }

	public ScenarioActionExecutor(DbScenarioAction action) : base(action) { }

	public override async Task<Dictionary<string, string>> Execute(IDictionary<string, string> currentContext)
	{
		base.Start();

		var loaded = await LoadActionsByScenarioGuidFunc(Action.CalledScenarioGuid);
		var runInfo = new ScenarioRunInfo
		{
			DbScenarioGuid = Action.CalledScenarioGuid,
			EntryPoint = loaded.EntryPoint,
			ActionsLoadedFromDb = loaded.Actions,
			ExecutionDepth = CallerExecutionDepth + 1,
			Parent = CallerGuid,
			LoadActionsByScenarioGuidFunc = this.LoadActionsByScenarioGuidFunc,
		};

		var execution = new ScenarioExecutor.ProjectInterface.ScenarioExecutor(runInfo);
		await execution.StartAsync();

		var js = $$$"""
			let scenarioResult = {{{Serialize(execution.Progress.CurrentVariableContext)}}};
		""";

		await ExecuteUserScript(currentContext, js);

		base.Complete();

		// перезапись процессорного времени - сколько занял вложенный сценарий
		_cpuTimeCounter.GetType().GetField("_elapsed", System.Reflection.BindingFlags.NonPublic)!
			.SetValue(_cpuTimeCounter, execution.Progress.ProcessorTicksCount);

		return new();
	}
}