using System;
using System.Diagnostics;
using System.Globalization;
using Models.Application.Abstract;
using Models.Application.TestScenarios.ActionResults;
using Models.Application.TestScenarios.ScenarioRun;
using Models.Database.Abstract;
using Models.Database.RunningScenarios;
using Models.Database.TestScenarios;
using ScenarioExecutor.ActionExecutors;
using ScenarioExecutor.Helpers;
using ScenarioExecutor.Interfaces;
using static System.Collections.Specialized.BitVector32;

namespace ScenarioExecutor.ProjectInterface;

public class ScenarioExecutor
{
	public const int MaxExecutionDepth = 16;

	public ScenarioProgressInfo Progress { get; init; }


	public ScenarioExecutor(ScenarioRunInfo runInfo)
	{
		if(runInfo.ExecutionDepth > MaxExecutionDepth)
			throw new AggregateException("Maximum execution depth reached.");

		Progress = new(runInfo);
	}


	public async Task StartAsync()
	{
		this.Progress.Started = DateTime.UtcNow;

		try
		{
			await ExecuteAsync();
		}
		catch(Exception ex)
		{
			this.Progress.Exception = ex;
		}

		this.Progress.Completed = DateTime.UtcNow;
	}

	private async Task ExecuteAsync()
	{
		ADbAction? action = null;
		Guid? actionGuid = FindEntryPoint();
		var actions = Progress.RunInfo.ActionsLoadedFromDb;

		while(actionGuid.HasValue)
		{
			if(!actions.TryGetValue(actionGuid.Value, out action))
				break;

			var executor = AActionExecutor.Create(action);
			if(executor is ScenarioActionExecutor sae)
			{
				sae.CallerGuid = actionGuid.Value;
				sae.CallerExecutionDepth = Progress.RunInfo.ExecutionDepth;
				sae.LoadActionsByScenarioGuidFunc = Progress.RunInfo.LoadActionsByScenarioGuidFunc;
			}

			this.Progress.ExecutionCount++;

			Dictionary<string, string>? updates = null;
			try { updates = await executor.Execute(this.Progress.CurrentVariableContext); }
			catch(Exception ex)
			{
				ValidateActionResult(action, executor.AbstractResult);
				throw;
			}

			var resultNullable = executor.AbstractResult;

			this.Progress.ProcessorTicksCount += executor.CpuTimeTicks;
			var result = ValidateActionResult(action, resultNullable);

			this.Progress.ActionResults.Add(result);

			// TODO: update context here
			this.Progress.CurrentVariableContext =
				ContextHelper.MergeContexts(this.Progress.CurrentVariableContext, updates);

			actionGuid = executor.AbstractResult?.Next
				?? actions.Values.Where(x => x.ColumnId == action.ColumnId)
				.Where(x => x.RowId > action.RowId).MinBy(x => x.RowId)?.Guid;
		}
	}

	private Guid FindEntryPoint()
	{
		var actions = Progress.RunInfo.ActionsLoadedFromDb;
		var ep = Progress.RunInfo.EntryPoint;
		ADbAction? action = null;

		if(actions.Count < 1)
			throw new Exception("Scenario contains no actions.");

		if(ep.HasValue && ep.Value != Guid.Empty && (!actions.TryGetValue(ep.Value, out action) || action is null))
			throw new Exception("Entry point action not found.");

		return action?.Guid ?? actions!
			.OrderBy(x => x.Value.ColumnId)
			.ThenBy(x => x.Value.RowId)
			.First().Value.Guid;
	}

	private AActionResult ValidateActionResult(ADbAction a, AActionResult? r)
	{
		var errLogs = string.Join(',', r?.Logs.Where(x => x.typ == Models.Enums.LogType.Error).Select(x => $"\'x.msg\'").ToList() ?? [""]);
		if(r is null || r.IsError || errLogs.Length > 2) throw new AggregateException(string.Join("\r\n",
			$"Failed to execute action: \'{a.Name}\'.",
			$"Action GUID: \'{a.Guid}\'.",
			$"Scenario GUID: \'{Progress.RunInfo.DbScenarioGuid}\'.",
			$"Scenario run GUID: \'{Progress.RunInfo.Guid}\'.",
			$"Parent GUID: \'{(Progress.RunInfo.Parent?.ToString() ?? "")}\'.",
			$"Execution Depth: \'{Progress.RunInfo.ExecutionDepth}\'",
			$"Timestamp: \'{DateTime.UtcNow.ToString("O", CultureInfo.InvariantCulture)}\'.",
			$"ErrorLogs: [{errLogs}]",
			$"IsError: \'{r?.IsError}\'."));

		return r;
	}
}