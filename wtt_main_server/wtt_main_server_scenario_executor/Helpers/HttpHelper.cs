using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using wtt_main_server_data.Database.TestExecutors;
using System.Net.Http.Json;
using Duende.IdentityServer.Models;
using System.Reflection.PortableExecutable;
using wtt_main_server_data.Database.TestScenarios;
using static System.Text.Json.JsonSerializer;
using static System.Collections.Specialized.BitVector32;
using Jint;
using wtt_main_server_data.Enums;

namespace wtt_main_server_scenario_executor.Helpers;

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
			_ => throw new NotImplementedException(),
		};
	}

	public static HttpClient GetWebClient(HttpClientSettings settings)
	{
		HttpClient client = settings.TlsValidationMode switch
		{
			HttpTlsValidationMode.Enabled => HttpClientHolder.WithAllValidations,
			HttpTlsValidationMode.AllowSelfSigned => HttpClientHolder.WithAllowedSelfSigned,
			HttpTlsValidationMode.Disabled => HttpClientHolder.WithAllowedNoTls,
			_ => throw new NotImplementedException(),
		};

		return client;
	}
}