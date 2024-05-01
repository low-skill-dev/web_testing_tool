using System.Net;
using System.Net.NetworkInformation;
using System.Reflection.PortableExecutable;
using Models.Application.TestScenarios.ActionResults;
using Models.Database.TestScenarios;
using ScenarioExecutor.Interfaces;
using static ScenarioExecutor.Helpers.ContextHelper;

namespace ScenarioExecutor.ActionExecutors;

public sealed class EchoActionExecutor : AActionExecutor<DbEchoAction, EchoActionResult>
{
	public EchoActionExecutor(DbEchoAction action) : base(action) { }

	public override async Task<Dictionary<string, string>> Execute(IDictionary<string, string> currentContext)
	{
		_cpuTimeCounter.Start();

		Result = new()
		{
			Started = DateTime.UtcNow
		};

		var (reply, delay) = await MakeRequestAsync(currentContext);
		Result.IsError = reply?.Status != 0;
		Result.PingDelayMs = delay.HasValue 
			? Convert.ToInt32(delay.Value.TotalMilliseconds) : -1;

		var js = $"let pingMs = {Convert.ToInt32(delay?.TotalMilliseconds ?? -1)};";
		await ExecuteUserScripts(currentContext, js);

		var ret = (this.Result!.ContextUpdates as IEnumerable<(string n, string v)>).Reverse().DistinctBy(x => x.n).ToDictionary(x => x.n, x => x.v);

		_cpuTimeCounter.Stop();
		Result.Completed = DateTime.UtcNow;
		return ret;
	}

	private async Task<(PingReply reply, TimeSpan? delay)> MakeRequestAsync(IDictionary<string, string> currentContext)
	{
		var url = CreateStringFromContext(Action.RequestUrl, currentContext);

		_cpuTimeCounter.Stop();
		var reply = await new Ping().SendPingAsync(url);
		_cpuTimeCounter.Start();

		return (reply, reply.Status == IPStatus.Success
			? TimeSpan.FromMilliseconds(reply.RoundtripTime) : null);
	}
}