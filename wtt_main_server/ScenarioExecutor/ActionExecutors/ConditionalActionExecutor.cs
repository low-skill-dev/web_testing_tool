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
using ScenarioExecutor.Helpers;
using ScenarioExecutor.Interfaces;
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

public sealed class ConditionalActionExecutor : AActionExecutor<DbConditionalAction, ConditionalActionResult>
{
	public ConditionalActionExecutor(DbConditionalAction action) : base(action) { }

	public override async Task<Dictionary<string, string>> Execute(IDictionary<string, string> currentContext)
	{
		base.Start();

		var condition = CreateStringFromContext(Action.JsBoolExpression, currentContext);
		bool? ifResult = null;

		var js = $$$"""
			if({{{condition}}}) resultTrue();
			else resultFalse();
		""";

		JsHelper.Execute(e =>
		{
			e.SetValue("resultTrue", () => ifResult = true);
			e.SetValue("resultFalse", () => ifResult = false);
		}, js);

		Result.IsError = !ifResult.HasValue;
		Result.Next = (ifResult ?? false) ? Action.ActionOnTrue : Action.ActionOnFalse;

		await ExecuteUserScript(currentContext);

		var ret = (this.Result!.ContextUpdates as IEnumerable<(string n, string v)>).Reverse().DistinctBy(x => x.n).ToDictionary(x => x.n, x => x.v);

		base.Complete();
		return ret;
	}
}