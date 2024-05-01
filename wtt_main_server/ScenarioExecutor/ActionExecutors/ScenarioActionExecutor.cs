using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Application.Abstract;
using Models.Application.TestScenarios.ActionResults;
using Models.Database.Abstract;
using Models.Database.Common;
using Models.Database.TestScenarios;
using ScenarioExecutor.Interfaces;
using static System.Collections.Specialized.BitVector32;

namespace ScenarioExecutor.ActionExecutors;

/// <summary>
/// Действие для вызова другого сценария из текущего
/// </summary>
public sealed class ScenarioActionExecutor : AActionExecutor<DbScenarioAction, ScenarioActionResult>
{ 
	public ScenarioActionExecutor(DbScenarioAction action) : base(action) { }

	public override async Task<Dictionary<string, string>> Execute(IDictionary<string, string> currentContext)
	{
		throw new NotImplementedException();
	}
}