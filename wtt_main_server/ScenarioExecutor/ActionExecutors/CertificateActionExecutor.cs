using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Models.Application.Abstract;
using Models.Application.TestScenarios.ActionResults;
using Models.Database.Abstract;
using Models.Database.Common;
using Models.Database.TestExecutors;
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
using System.Net;

namespace ScenarioExecutor.ActionExecutors;


public class CertificateActionExecutor : AActionExecutor<DbCertificateAction, CertificateActionResult>
{
	public CertificateActionExecutor(DbCertificateAction action) : base(action) { }

	public override async Task<Dictionary<string, string>> Execute(IDictionary<string, string> currentContext)
	{
		_cpuTimeCounter.Start();

		Result = new()
		{
			Started = DateTime.UtcNow
		};

		var cert = await MakeRequestAsync(currentContext);
		Result.RetrievedCertificate = cert;
		Result.IsError = cert is null;

		await ExecuteUserScripts(currentContext);

		var ret = (this.Result!.ContextUpdates as IEnumerable<(string n, string v)>).Reverse().DistinctBy(x => x.n).ToDictionary(x => x.n, x => x.v);

		_cpuTimeCounter.Stop();
		Result.Completed = DateTime.UtcNow;
		return ret;
	}

	private async Task<X509Certificate?> MakeRequestAsync(IDictionary<string, string> currentContext)
	{
		var url = CreateStringFromContext(Action.RequestUrl, currentContext);
		var client = HttpHelper.GetWebClient();

		try
		{
#pragma warning disable SYSLIB0014
			var request = (HttpWebRequest)WebRequest.Create(url);
			_cpuTimeCounter.Stop();
			await request.GetResponseAsync();
			_cpuTimeCounter.Start();
			var cert = request.ServicePoint.Certificate;
			return cert;
		}
		catch
		{
			return null;
		}
	}
}
