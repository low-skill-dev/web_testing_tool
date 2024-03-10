using System;
using System.Diagnostics;
using System.Globalization;
using Models.Application.TestScenarios.ActionResults;
using Models.Application.TestScenarios.ScenarioRun;
using Models.Database.Abstract;
using Models.Database.RunningScenarios;
using Models.Database.TestScenarios;
using ScenarioExecutor.ActionExecutors;
using ScenarioExecutor.Interfaces;

namespace ScenarioExecutor.ProjectInterface;

public class ScenarioExecutor
{
	public const int MaxExecutionDepth = 16;

	public ScenarioProgressInfo Progress { get; init; }


	public ScenarioExecutor(ScenarioRunInfo runInfo, long executionDepth = 0)
	{
		if(executionDepth > MaxExecutionDepth)
			throw new AggregateException("Maximum execution depth reached.");

		Progress = new(runInfo);
	}


	public async Task StartAsync()
	{
		this.Progress.Started = DateTime.UtcNow;

		try
		{
			Guid? actionGuid = Progress.RunInfo.EntryPoint;

			while(actionGuid.HasValue)
			{
				var action = Progress.RunInfo.ActionsLoadedFromDb[actionGuid.Value];

				if(action is null) break;

				var executor = AActionExecutor.Create(action, Progress.RunInfo.DbExecutionLimitations);

				this.Progress.ExecutionCount++;
				await executor.Execute(this.Progress.CurrentVariableContext);

				var result = executor.AbstractResult;

				if(result is null) throw new AggregateException(string.Join(' ',
					$"Failed to execute action \'{action.Name}\'.",
					$"Action GUID: \'{actionGuid}\'.",
					$"Scenario GUID: \'{Progress.RunInfo.DbScenarioGuid}\'.",
					$"Scenario run GUID: \'{Progress.RunInfo.Guid}\'.",
					$"Timestamp: \'{DateTime.UtcNow.ToString("O", CultureInfo.InvariantCulture)}\'."));

				this.Progress.ActionResults.Add(result);

				// TODO: update context here

				actionGuid = executor.AbstractResult?.Next;
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

		this.Progress.ProcessorTime = this.Progress.ActionResults
			.Where(x => x is not DelayActionResult)
			.Select(x => x.Completed - x.Started)
			.Aggregate((x, y) => x + y);

		await Task.CompletedTask;
	}
}