using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Models.Database.TestExecutors;
using System.Net.Http.Json;
using System.Reflection.PortableExecutable;
using Models.Database.TestScenarios;
using static System.Text.Json.JsonSerializer;
using static System.Collections.Specialized.BitVector32;
using Jint;
using Models.Enums;

namespace ScenarioExecutor.Helpers;

internal static class HttpHelper
{
	private static int _maxParallelRequests = 64;
	private static int _currentActiveRequests = 0;
	private static int _totalCompletedRequests = 0;

	public static int MaxParallelRequests
	{
		get
		{
			return _maxParallelRequests;
		}
		set
		{
			if(_maxParallelRequests < 1) throw new
				ArgumentOutOfRangeException(nameof(MaxParallelRequests));

			_maxParallelRequests = value;
		}
	}
	public static int CurrentActiveRequests => _currentActiveRequests;
	public static int TotalCompletedRequests => _totalCompletedRequests;


	public static HttpMethod ToNetHttpMethod(this HttpRequestMethod method)
	{
		return method switch
		{
			HttpRequestMethod.Get => HttpMethod.Get,
			HttpRequestMethod.Post => HttpMethod.Post,
			HttpRequestMethod.Put => HttpMethod.Put,
			HttpRequestMethod.Patch => HttpMethod.Patch,
			HttpRequestMethod.Delete => HttpMethod.Delete,
			//_ => throw new NotImplementedException(),
			_ => HttpMethod.Get,
		};
	}

	public static HttpClient GetWebClient(HttpClientSettings? settings = null)
	{
		HttpClient client = settings?.TlsValidationMode switch
		{
			HttpTlsValidationMode.AllowSelfSigned => HttpClientHolder.WithAllowedSelfSigned,
			HttpTlsValidationMode.Disabled => HttpClientHolder.WithAllowedNoTls,
			HttpTlsValidationMode.Enabled or _ => HttpClientHolder.WithAllValidations,
		};

		return HttpClientHolder.WithAllowedNoTls;
	}


	const int _maxProxiedClients = 32;
	static Dictionary<int, HttpClient> proxyToClient = new(_maxProxiedClients);
	public static HttpClient GetWebClient(string proxyUrl,
		string proxyUsername = "", string proxyPassword = "")
	{
		var proxyHash = HashCode.Combine(proxyUrl, proxyUsername, proxyPassword);
		if(proxyToClient.TryGetValue(proxyHash, out var v1)) return v1;

		lock(proxyToClient)
		{
			if(proxyToClient.TryGetValue(proxyHash, out var v2)) return v2;
			if(proxyToClient.Count == _maxProxiedClients) CleanProxiedClients();

			var c = new HttpClient(new HttpClientHandler
			{
				UseProxy = true,
				Proxy = new WebProxy
				{
					Address = new(proxyUrl),
					Credentials = string.IsNullOrWhiteSpace(proxyUsername + proxyPassword)
						? null : new NetworkCredential
						{
							UserName = proxyUsername,
							Password = proxyPassword,
						},
				},
			});

			proxyToClient.Add(proxyHash, c);
			return c;
		}
	}

	private static void CleanProxiedClients()
	{
		var clients = proxyToClient.Values;
		proxyToClient.Clear();

		_ = Task.Run(async () =>
		{
			await Task.Delay(60_000);
			foreach(var c in clients) c.Dispose();
		});
	}
}