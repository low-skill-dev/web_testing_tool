using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Application.Abstract;
using Models.Application.TestScenarios.ActionResults;
using Models.Application.TestScenarios.Parameter;
using Models.Database.Abstract;
using Models.Database.Common;
using Models.Database.TestScenarios;
using ScenarioExecutor.Interfaces;

namespace ScenarioExecutor.ActionExecutors;

public sealed class ErrorActionExecutor : AActionExecutor<DbErrorAction, ErrorActionResult>
{
	public ErrorActionExecutor(DbErrorAction action) : base(action) { }

	public override async Task Execute(IDictionary<string, string> currentContext)
	{
		throw new NotImplementedException();
	}
}