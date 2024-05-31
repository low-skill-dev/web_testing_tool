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
using System.Collections.Concurrent;
using System.Runtime.ConstrainedExecution;
using Models.Enums;

namespace ScenarioExecutor.ActionExecutors;


public class CertificateActionExecutor : AActionExecutor<DbCertificateAction, CertificateActionResult>
{
	public CertificateActionExecutor(DbCertificateAction action) : base(action) { }

	public override async Task<Dictionary<string, string>> Execute(IDictionary<string, string> currentContext)
	{
		base.Start();

		var cert = await MakeRequest(currentContext);
		Result.RetrievedCertificate = cert;

		if(cert is null)
		{
			Result.IsError = true;
		}
		else if(cert.NotAfter < DateTime.UtcNow.AddDays(Action.MinimalDaysRemaining))
		{
			Result.IsError = true;
			Result.Logs.Add((LogType.Error, "Certificate has expired."));
		}

		await ExecuteUserScript(currentContext);

		var ret = (this.Result!.ContextUpdates as IEnumerable<(string n, string v)>).Reverse().DistinctBy(x => x.n).ToDictionary(x => x.n, x => x.v);

		base.Complete();
		return ret;
	}



	private static ConcurrentDictionary<string, X509Certificate2?> _results = new();
	private static HttpClient _client = new(new HttpClientHandler
	{
		ServerCertificateCustomValidationCallback = (req, cert, chain, policy) =>
		{
			if(req?.RequestUri?.AbsoluteUri is not null)
				_results[req.RequestUri.AbsoluteUri] = cert;
			return true;
		}
	});


	private async Task<X509Certificate2?> MakeRequest(IDictionary<string, string> currentContext)
	{
		var url = CreateStringFromContext(Action.RequestUrl, currentContext);
		var uri = new Uri(url);

		try
		{
			_results.TryAdd(uri.AbsoluteUri, null);
			_results[uri.AbsoluteUri] = null;
			var req = (await _client.GetAsync(uri)).RequestMessage!.RequestUri!.AbsoluteUri;
			return _results[uri.AbsoluteUri];
		}
		catch
		{
			return null;
		}
	}
}
