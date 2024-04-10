﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;
using Models.Application.Abstract;
using Models.Application.TestScenarios.Parameter;
using Models.Constants;
using Models.Database.Abstract;
using Models.Database.Common;
using Models.Database.TestScenarios;
using ScenarioExecutor.ActionExecutors;
using ScenarioExecutor.Helpers;

namespace ScenarioExecutor.Interfaces;

public abstract class AActionExecutor
{
	protected readonly Stopwatch _cpuTimeCounter = new();
	public long CpuTimeTicks => _cpuTimeCounter.ElapsedTicks;

	public virtual ADbAction AbstractAction { get; protected set; }
	public virtual AActionResult? AbstractResult { get; protected set; }
	public DbTariff? UserSubscription { get; private set; }

	protected AActionExecutor(ADbAction action)
	{
		this.AbstractAction = action;
	}

	public abstract Task Execute(IDictionary<string, string> currentContext);

	public static AActionExecutor Create(ADbAction action, DbTariff? subscription = null)
	{
		AActionExecutor? ae = null;

		if(action is DbHttpAction dbHttpAction) ae = new HttpActionExecutor(dbHttpAction);
		if(action is DbEchoAction dbEchoAction) ae = new EchoActionExecutor(dbEchoAction);
		if(action is DbImapAction dbImapAction) ae = new ImapActionExecutor(dbImapAction);
		if(action is DbDelayAction dbDelayAction) ae = new DelayActionExecutor(dbDelayAction);
		if(action is DbLogAction dbErrorAction) ae = new ErrorActionExecutor(dbErrorAction);
		if(action is DbScenarioAction dbScenarioAction) ae = new ScenarioActionExecutor(dbScenarioAction);
		if(action is DbConditionalAction dbConditionalAction) ae = new ConditionalActionExecutor(dbConditionalAction);

		if(ae is null) throw new NotImplementedException(nameof(ADbAction));

		ae.UserSubscription = subscription;

		return ae;
	}
}

public abstract class AActionExecutor<A, R> : AActionExecutor where A : ADbAction where R : AActionResult
{
	public A Action
	{
		get => (A)AbstractAction;
		set => AbstractAction = value;
	}
	public R? Result
	{
		get => (R?)base.AbstractResult;
		set => base.AbstractResult = value;
	}

	protected AActionExecutor(A action) : base(action) { }

	protected virtual async Task ExecuteUserScripts(IDictionary<string, string> context,
		string beforeContext = "",
		string beforeScript = "",
		string beforeUpdate = "")
	{
#pragma warning disable format // @formatter:off

		var ctxJs = string.Join('\n', context.Select(x => $"{x.Key} = {x.Value};"));
		var updJs = string.Join('\n', Action.VariableToPath?.Select(x =>
		{
			var errorCmd = $"{JsConsts.LogError}(\"Error parsing \'{x.Key}\' variable.\");";

			var l1 = $"try {{ {x.Key}={x.Value}; {JsConsts.UpdateVariable}({x.Key},{x.Value}); }}";
			var l2 = $"catch {{ {errorCmd} }}";
			var l3 = $"finally {{ }}";

			return string.Join('\n', l1, l2, l3);
		}) ?? Array.Empty<string>());

		var js = $$$"""
			
			// generated: {{{DateTime.UtcNow.ToString("O")}}}
			// action: {{{Action.Guid}}}

			// --- BEFORE UPDATE ---

			{{{beforeContext}}}

			{{{ctxJs}}}

			{{{beforeScript}}}
		
			{{{Action.AfterRunScript}}}

			{{{beforeUpdate}}}

			// --- UPDATE ---

			{{{updJs}}}
		""";

#pragma warning restore format // @formatter:on

		//var contextUpdates = new List<(string name, string val)>(Action.VariableToPath.Count);
		//var errors = new List<(string msg, bool crit)>(Action.VariableToPath.Count);

		//var updateVariableFunc = (string name, string val) => contextUpdates.Add((name, val));
		//var logErrorFunc = (string msg, bool crit = false) => errors.Add((msg, crit));

		JsHelper.Execute(eng => this.Result!.BindAll(eng), js, this.UserSubscription);
	}
}
