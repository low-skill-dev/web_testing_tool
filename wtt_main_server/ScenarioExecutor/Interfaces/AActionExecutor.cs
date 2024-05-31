using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;
using Models.Application.Abstract;
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

	public virtual ADbAction AbstractAction { get; set; }
	public virtual AActionResult? AbstractResult { get; set; }

	protected AActionExecutor(ADbAction action)
	{
		this.AbstractAction = action;
	}

	public abstract Task<Dictionary<string, string>> Execute(IDictionary<string, string> currentContext);

	public static AActionExecutor Create(ADbAction action)
	{
		AActionExecutor? ae = null;

		if(action is DbHttpAction dbHttpAction) ae = new HttpActionExecutor(dbHttpAction);
		if(action is DbEchoAction dbEchoAction) ae = new EchoActionExecutor(dbEchoAction);
		if(action is DbImapAction dbImapAction) ae = new ImapActionExecutor(dbImapAction);
		if(action is DbErrorAction dbErrorAction) ae = new ErrorActionExecutor(dbErrorAction);
		if(action is DbDelayAction dbDelayAction) ae = new DelayActionExecutor(dbDelayAction);
		if(action is DbScenarioAction dbScenarioAction) ae = new ScenarioActionExecutor(dbScenarioAction);
		if(action is DbCertificateAction dbCertificateAction) ae = new CertificateActionExecutor(dbCertificateAction);
		if(action is DbConditionalAction dbConditionalAction) ae = new ConditionalActionExecutor(dbConditionalAction);

		if(ae is null) throw new NotImplementedException(nameof(ADbAction));

		return ae;
	}

	protected virtual async Task ExecuteUserScript(IDictionary<string, string> context,
		string beforeContext = "", string beforeScript = "", string afterScript = "")
	{

		if(context.Count == 0
			&& string.IsNullOrWhiteSpace(beforeContext)
			&& string.IsNullOrWhiteSpace(beforeScript)
			&& string.IsNullOrWhiteSpace(afterScript)) return;

		var ctxJs = string.Join('\n', context.Select(x => $"let {x.Key} = '{x.Value}';"));

		var js = $$$"""		
			// generated: {{{DateTime.UtcNow.ToString("")}}}
			// action: {{{AbstractAction.Guid}}}

			// BEFORE CONTEXT

			{{{beforeContext}}}

			// CONTEXT

			{{{ctxJs}}}

			// BEFORE SCRIPT

			{{{beforeScript}}}

			// USER SCRIPT
		
			{{{AbstractAction.AfterRunScript}}}

			// AFTER SCRIPT

			{{{afterScript}}}
		""";

		await Task.Run(() => JsHelper.Execute(eng => this.AbstractResult!.BindAll(eng), js));
	}
}

public abstract class AActionExecutor<A, R> : AActionExecutor where A : ADbAction where R : AActionResult, new()
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

	protected void Start()
	{
		_cpuTimeCounter.Start();
		Result = new() { Started = DateTime.UtcNow };
	}

	protected void Complete()
	{
		_cpuTimeCounter.Stop();
		Result.Completed = DateTime.UtcNow;
	}
}
