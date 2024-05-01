using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using Models.Application.Abstract;
using Models.Application.TestScenarios.ActionResults;
using Models.Database.Abstract;
using Models.Database.Common;
using Models.Database.TestScenarios;
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

public sealed class ImapActionExecutor : AActionExecutor<DbImapAction, ImapActionResult>
{
	public ImapActionExecutor(DbImapAction action) : base(action) { }

	public override async Task<Dictionary<string, string>> Execute(IDictionary<string, string> currentContext)
	{
		_cpuTimeCounter.Start();

		Result = new()
		{
			Started = DateTime.UtcNow
		};

		
	}

	private async Task<(PingReply reply, TimeSpan? delay)> MakeRequestAsync(IDictionary<string, string> currentContext)
	{
		var userName = CreateStr
	}
}
