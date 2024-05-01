using System;
using System.Diagnostics;
using System.Globalization;
using Models.Application.TestScenarios.ActionResults;
using Models.Application.TestScenarios.ScenarioRun;
using Models.Database.Abstract;
using Models.Database.RunningScenarios;
using Models.Database.TestScenarios;
using ScenarioExecutor.ActionExecutors;
using ScenarioExecutor.Helpers;
using ScenarioExecutor.Interfaces;

namespace ScenarioExecutor.ProjectInterface;

public class ScenarioExecution
{
	public const int MaxExecutionDepth = 16;

	public ScenarioProgressInfo Progress { get; init; }


	public ScenarioExecution(ScenarioRunInfo runInfo)
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
			var ep = Progress.RunInfo.EntryPoint;
			var actions = Progress.RunInfo.ActionsLoadedFromDb;
			ADbAction? action = null;

			if(actions.Count < 1)
			{
				throw new Exception("Scenario contains no actions.");
			}

			if(ep.HasValue && !actions.TryGetValue(ep.Value, out action))
			{
				throw new Exception("Entry point action not found.");
			}

			Guid? actionGuid = action?.Guid ?? actions!
				.OrderByDescending(x => x.Value.ColumnId)
				.ThenByDescending(x => x.Value.RowId)
				.First().Value.Guid;


			while(actionGuid.HasValue)
			{
				if(!actions.TryGetValue(actionGuid.Value, out action))
					break;

				var executor = AActionExecutor.Create(action);

				this.Progress.ExecutionCount++;

				var updates = await executor.Execute(this.Progress.CurrentVariableContext);
				var result = executor.AbstractResult;

				if(result is null || result.IsError) throw new AggregateException(string.Join('\n',
					$"Failed to execute action \'{action.Name}\'.",
					$"Action GUID: \'{actionGuid}\'.",
					$"Scenario GUID: \'{Progress.RunInfo.DbScenarioGuid}\'.",
					$"Scenario run GUID: \'{Progress.RunInfo.Guid}\'.",
					$"Timestamp: \'{DateTime.UtcNow.ToString("O", CultureInfo.InvariantCulture)}\'.",
					$"IsError: \'{result?.IsError}\'."));

				this.Progress.ActionResults.Add(result);

				// TODO: add logs

				// TODO: update context here
				this.Progress.CurrentVariableContext =
					ContextHelper.MergeContexts(this.Progress.CurrentVariableContext, updates);

				actionGuid = executor.AbstractResult?.Next
					?? actions.Values.Where(x => x.ColumnId == action.ColumnId)
					.Where(x => x.RowId > action.RowId).MinBy(x => x.RowId)?.Guid;			
			}
		}
		catch(Exception ex)
		{
			this.Progress.Exception = ex;
		}

		await CompleteAsync();
	}

	private async Task CompleteAsync()
	{
		this.Progress.Completed = DateTime.UtcNow;

		await Task.CompletedTask;
	}
}