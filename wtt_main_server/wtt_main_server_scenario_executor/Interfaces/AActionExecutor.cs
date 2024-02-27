using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;
using wtt_main_server_data.Application.Abstract;
using wtt_main_server_data.Application.TestScenarios.Parameter;
using wtt_main_server_data.Database.Abstract;
using wtt_main_server_data.Database.Common;
using wtt_main_server_data.Database.TestScenarios;
using wtt_main_server_scenario_executor.ActionExecutors;

namespace wtt_main_server_scenario_executor.Interfaces;

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

		if(action is DbHttpAction dbHttpAction) ae = new HttpActionExecutor(dbHttpAction) { Tariff = subscription ?? new() };
		if(action is DbEchoAction dbEchoAction) ae = new EchoActionExecutor(dbEchoAction);
		if(action is DbImapAction dbImapAction) ae = new ImapActionExecutor(dbImapAction);
		if(action is DbDelayAction dbDelayAction) ae = new DelayActionExecutor(dbDelayAction);
		if(action is DbErrorAction dbErrorAction) ae = new ErrorActionExecutor(dbErrorAction);
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
}
