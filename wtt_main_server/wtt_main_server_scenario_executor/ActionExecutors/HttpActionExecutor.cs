using Jint;
using System.Text;
using wtt_main_server_data.Application.Abstract;
using wtt_main_server_data.Application.TestScenarios.ActionResults;
using wtt_main_server_data.Database.Abstract;
using wtt_main_server_data.Database.Common;
using wtt_main_server_data.Database.TestScenarios;
using wtt_main_server_data.Database.TestExecutors;
using wtt_main_server_scenario_executor.Helpers;
using wtt_main_server_scenario_executor.Interfaces;
using static wtt_main_server_scenario_executor.Helpers.ContextHelper;
using System.Diagnostics;
using static System.Text.Json.JsonSerializer;
using System;
using wtt_main_server_data.Constants;
using Common.Helpers;

namespace wtt_main_server_scenario_executor.ActionExecutors;

public sealed class HttpActionExecutor : AActionExecutor<DbHttpAction, HttpActionResult>
{
	#region static

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

	#endregion

	public HttpActionExecutor(DbHttpAction action) : base(action) { }

	/// <returns>
	/// Context updates, also present in ActionResult
	/// </returns>
	public override async Task<Dictionary<string, string>> Execute(IDictionary<string, string> currentContext)
	{
		_cpuTimeCounter.Start();

		Result = new()
		{
			Started = DateTime.UtcNow
		};

		var (req, res) = await MakeRequestAsync(currentContext);

		await ExecuteUserScripts(currentContext, req, res);

		var ret = (this.Result!.ContextUpdates as IEnumerable<(string n, string v)>).Reverse().DistinctBy(x => x.n).ToDictionary(x => x.n, x => x.v);

		_cpuTimeCounter.Stop();
		Result.Completed = DateTime.UtcNow;
		return ret;
	}

	#region private

	private async Task<(HttpRequestMessage req, HttpResponseMessage res)> MakeRequestAsync(IDictionary<string, string> currentContext)
	{
		var client = HttpHelper.GetWebClient(new HttpClientSettings
		{
			TlsValidationMode = this.Action.TlsValidationMode
		});

		var url = CreateStringFromContext(Action.RequestUrl, currentContext);

		var body = Action.RequestBody is null ? null :
			CreateStringFromContext(Action.RequestBody, currentContext);

		var cookies = Action.RequestCookies?.Select(x => KeyValuePair.Create(
			x.Key, CreateStringFromContext(x.Value, currentContext)))
			.ToDictionary(x => x.Key, x => x.Value);

		var headers = Action.RequestHeaders?.Select(x => KeyValuePair.Create(
			x.Key, CreateStringFromContext(x.Value, currentContext)))
			.ToDictionary(x => x.Key, x => x.Value);

		var msg = new HttpRequestMessage(this.Action.Method.ToNetHttpMethod(), new Uri(url));

		msg.Content = body is null ? new ByteArrayContent(Array.Empty<byte>())
			: msg.Content = new ByteArrayContent(Encoding.UTF8.GetBytes(body));
		if(cookies is not null) msg.Content.Headers.Add("Cookie", string.Join(';', cookies.Select(x => $"{x.Key}={x.Value}")));
		if(headers is not null) foreach(var pair in headers) msg.Content.Headers.Add(pair.Key, pair.Value);

		_cpuTimeCounter.Stop();
		int safeCounter = 0;
		while(_currentActiveRequests >= _maxParallelRequests && safeCounter++ < 300)
		{
			await Task.Delay(5000);
		}
		_cpuTimeCounter.Start();

		try
		{
			Interlocked.Increment(ref _currentActiveRequests);

			_cpuTimeCounter.Stop();
			var response = await client.SendAsync(msg, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);
			_cpuTimeCounter.Start();

			return (msg, response);
		}
		finally
		{
			Interlocked.Decrement(ref _currentActiveRequests);
			Interlocked.Increment(ref _totalCompletedRequests);
		}
	}

	/// <returns>
	/// A context that should be merged with current to create 
	/// context for the next action;
	/// </returns>
	private async Task ExecuteUserScripts(IDictionary<string, string> context, HttpRequestMessage request, HttpResponseMessage response)
	{
#pragma warning disable format // @formatter:off

		var reqJs = await GetMessageAsJsVariable(HttpType.Request, request);
		var resJs = await GetMessageAsJsVariable(HttpType.Response, response);
		var ctxJs = string.Join('\n', context.Select(x => $"{x.Key} = {x.Value};"));
		var updJs = string.Join('\n', Action.VariableToPath?.Select(x =>
		{
			var nonCritical = !Action.VariablesUpdatedInTryBlock?.Any(v => v.Equals(x.Key)) ?? false;
			var errorCmd = $"{(nonCritical ? JsConsts.LogWarn : JsConsts.LogError)}({nonCritical.ToString().ToLower()}, 'Error parsing \'{x.Key}\' variable value.');";

			var l1 = $"try {{ {x.Key}={x.Value}; {JsConsts.UpdateVariable}({x.Key},{x.Value}); }}";
			var l2 = $"catch {{ {errorCmd} }}";
			var l3 = $"finally {{ }}";

			return string.Join('\n', l1, l2, l3);
		}) ?? Array.Empty<string>());

		var js = $$$"""
			
			// generated: {{{DateTime.UtcNow.ToString("O")}}}
			// action: {{{Action.Guid}}}

			// --- BEFORE UPDATE ---

			{{{reqJs}}}
			{{{resJs}}}

			{{{ctxJs}}}
		
			{{{Action.UserScript}}}

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

	private enum HttpType
	{
		Request,
		Response,
	}

	// TODO: replace dynamic with 2 types in dotnet 9/10
	private static async Task<string> GetMessageAsJsVariable(HttpType type, dynamic msg)
	{
		if(!(msg is HttpRequestMessage or HttpResponseMessage)) throw new ArgumentException(nameof(msg));

		string body = msg.Content is not null ? (await msg.Content.ReadAsStringAsync().ConfigureAwait(false)) : string.Empty;

		/* The Cookie spec (RFC 2109) does claim that you can combine multiple 
		 * cookies in one header the same way other headers can be combined (comma-separated), 
		 * but it also points out that non-conforming syntaxes (like the Expires parameter, 
		 * which has ,s in its value) are still common and must be dealt with by implementations.
		 * 
		 * RFC 2109 has been obsoleted by RFC 2965 that in turn got obsoleted by RFC 6265, which is stricter on the issue:
		 * Origin servers SHOULD NOT fold multiple Set-Cookie header fields into a single header field. The usual mechanism 
		 * for folding HTTP headers fields (i.e., as defined in [RFC2616]) might change the semantics of the Set-Cookie 
		 * header field because the %x2C (",") character is used by Set-Cookie in a way that conflicts with such folding.
		 * 
		 * https://datatracker.ietf.org/doc/html/rfc6265#section-3
		 * https://datatracker.ietf.org/doc/html/rfc2616#section-4.2
		 */


		msg.Headers.TryGetValues("Set-Cookie", out IEnumerable<string> t);
		var cookies = t is null ? new Dictionary<string, string>()
			: t.Select(x => x.Split(';', 2)[0].Split('=', 2)).ToDictionary(x => x[0], x => x[1]);

		Dictionary<string, List<string>> headers = new();
		foreach(var h in msg.Headers)
		{
			if(h.Key == "Set-Cookie") continue;

			if(!headers.ContainsKey(h.Key)) headers.Add(h.Key, new List<string>());
			foreach(var v in h.Value) headers[h.Key].Add(v);
		}

		var req_res = type == HttpType.Request ? "req" : "res";
		var request_response = type == HttpType.Request ? "request" : "response";

		bool bodyAsJson;
		if(!(body.StartsWith('{') && body.EndsWith('}')))
		{
			bodyAsJson = false;
			StringHelper.RemoveNewLines(body);
		}
		else
		{
			bodyAsJson = true;
		}

		var js = $$$"""
			
			let {{{req_res}}}Body = {{{(bodyAsJson ? body : $"\"{body}\"")}}};
			let {{{req_res}}}Cookies = {{{Serialize(cookies)}}};
			let {{{req_res}}}Headers = {{{Serialize(headers)}}};

			let {{{request_response}}} = {
				'body': {{{req_res}}}Body,
				'cookies': {{{req_res}}}Cookies,
				'headers': {{{req_res}}}Headers
			};

		""";

		return js;
	}

	private static async Task<string> GetMessageAsJsVariable(HttpResponseMessage msg)
	{
		var body = msg.Content is not null ? (await msg.Content.ReadAsStringAsync().ConfigureAwait(false)) : string.Empty;

		msg.Headers.TryGetValues("Set-Cookie", out var t);
		var cookies = t is null ? new Dictionary<string, string>()
			: t.Select(x => x.Split(';', 2)[0].Split('=', 2)).ToDictionary(x => x[0], x => x[1]);

		var headers = msg.Headers.ToDictionary(x => x.Key, x => x.Value.Single());

		var js = $$$"""
			
			let resBody = '{{{body}}}';
			let resCookies = {{{Serialize(cookies)}}};
			let resHeaders = {{{Serialize(headers)}}};

			let response = {
				'body': resBody,
				'cookies': resCookies,
				'headers': resHeaders
			};

		""";

		return js;
	}

	#endregion

}
