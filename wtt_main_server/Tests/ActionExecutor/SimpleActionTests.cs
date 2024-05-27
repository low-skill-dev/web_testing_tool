using Models.Database.TestScenarios;
using ScenarioExecutor.ActionExecutors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestHelper;
using Xunit.Abstractions;

namespace Tests.ActionExecutor;
public class SimpleActionTests : ATestsWithLogging
{
	private Dictionary<string, string> _emptyContext = new();
	public SimpleActionTests(ITestOutputHelper o) : base(o) { }

	[Fact]
	public async Task Create()
	{
		Assert.NotNull(new HttpActionExecutor(new()));
		Assert.NotNull(new EchoActionExecutor(new()));
		Assert.NotNull(new CertificateActionExecutor(new()));
		Assert.NotNull(new ConditionalActionExecutor(new()));
		Assert.NotNull(new DelayActionExecutor(new()));
		Assert.NotNull(new ErrorActionExecutor(new()));
		Assert.NotNull(new ImapActionExecutor(new()));
		Assert.NotNull(new ScenarioActionExecutor(new()));
	}

	[Fact]
	public async Task SimpleHttp()
	{
		var executor = new HttpActionExecutor(new DbHttpAction()
		{
			Guid = Guid.NewGuid(),
			Method = Models.Enums.HttpRequestMethod.Get,
			RequestUrl = @"https://jsonplaceholder.typicode.com/posts/1",
		});

		var result = await executor.Execute(_emptyContext);

		Assert.NotNull(result);
		Assert.NotNull(executor.Result!);
		Assert.NotEmpty(executor.Result.ResponseBody);
		Assert.StartsWith("{", executor.Result.ResponseBody);
		Assert.True(executor.CpuTimeTicks > 0);
	}

	[Fact]
	public async Task SimpleHttpProxied()
	{
		var arr = new object[2048];

		// MaxDegreeOfParallelism = default :: 37 sec
		// MaxDegreeOfParallelism = 48		:: 10 sec
		// MaxDegreeOfParallelism = 64		:: 8 sec	<- best
		// MaxDegreeOfParallelism = 96		:: 11 sec
		await Parallel.ForEachAsync(arr, new ParallelOptions()
		{
			MaxDegreeOfParallelism = 64,
		}, async (x, _) =>
		{
			var executor = new HttpActionExecutor(new DbHttpAction()
			{
				Guid = Guid.NewGuid(),
				Method = Models.Enums.HttpRequestMethod.Get,
				RequestUrl = @"https://api.ipify.org?format=json",
				ProxyUrl = "socks5://5.42.95.199:59099",
				ProxyUsername = "rrv",
				ProxyPassword = "BtkmjOfpbqWqHq1",
			});

			var result = await executor.Execute(_emptyContext);

			Assert.Contains("5.42.95.199", executor.Result.ResponseBody);
			Assert.Equal("{\"ip\":\"5.42.95.199\"}", executor.Result.ResponseBody);
		});
	}

	[Fact]
	public async Task SimpleEcho()
	{
		string[] urls = [
			@"https://jsonplaceholder.typicode.com/posts/1",
			@"https://www.google.com",
			@"https://www.ya.ru",
			@"https://vdb.lowskill.dev",
		];

		var executors = urls.Select(u =>
			new EchoActionExecutor(new DbEchoAction()
			{
				Guid = Guid.NewGuid(),
				RequestUrl = u.Split("//")[1].Split("/")[0],
			})).ToList();

		List<Task> tasks = new(urls.Length);

		foreach(var a in executors)
			tasks.Add(a.Execute(_emptyContext));

		await Task.WhenAll(tasks);

		Assert.True(executors.All(x => (x.Result?.PingDelayMs ?? -1) > 0));
		Assert.True(executors.All(x => (x.Result?.IsError ?? true) == false));
		Assert.True(executors.All(x => x.CpuTimeTicks > 0));
	}

	[Fact]
	public async Task SimpleImap()
	{
		var executor = new ImapActionExecutor(new DbImapAction()
		{
			Guid = Guid.NewGuid(),
			SenderMustContain = "security@id.mail.ru",
			BodySearchRegex = "з.*ь а.*нт",
		});

		var result = await executor.Execute(_emptyContext);

		Assert.NotNull(result);
		Assert.NotNull(executor.Result!);
		Assert.Equal("защитить аккаунт", executor.Result.FoundValue);
		Assert.True(executor.CpuTimeTicks > 0);

		executor = new ImapActionExecutor(new DbImapAction()
		{
			Guid = Guid.NewGuid(),
			SenderMustContain = "security@id.mail.ru",
			BodySearchRegex = "з.*ьTа.*нт",
		});

		result = await executor.Execute(_emptyContext);

		Assert.NotNull(result);
		Assert.NotNull(executor.Result!);
		Assert.Null(executor.Result.FoundValue);
		Assert.True(executor.Result.IsError);
	}

	[Fact]
	public async Task SimpleX509()
	{
		var executor = new CertificateActionExecutor(new DbCertificateAction()
		{
			Guid = Guid.NewGuid(),
			RequestUrl = "https://vdb.lowskill.dev",
		});

		var result = await executor.Execute(_emptyContext);

		Assert.NotNull(result);
		Assert.NotNull(executor.Result.RetrievedCertificate);
		Assert.Contains("Google Trust Services", executor.Result.RetrievedCertificate.Issuer);
	}


	[Fact]
	public async Task SimpleDelay()
	{
		var executor = new DelayActionExecutor(new DbDelayAction()
		{
			Guid = Guid.NewGuid(),
			DelaySeconds = 2,
		});

		var start = DateTime.UtcNow;
		var result = await executor.Execute(_emptyContext);
		var total = (DateTime.UtcNow - start).TotalMilliseconds;

		Assert.NotNull(result);
		Assert.False(executor.Result.IsError);
		Assert.InRange(total, 900, 1100);
	}

	[Fact]
	public async Task SimpleError()
	{
		var executor = new ErrorActionExecutor(new DbErrorAction()
		{
			Guid = Guid.NewGuid(),
		});

		var result = await executor.Execute(_emptyContext);

		Assert.NotNull(result);
		Assert.True(executor.Result.IsError);
	}

	[Fact]
	public async Task SimpleConditional()
	{
		var context = new Dictionary<string, string>()
		{
			{ "var1", "5" },
			{ "var2", "10" },
		};

		var trueGuid = Guid.NewGuid();
		var falseGuid = Guid.NewGuid();

		var executor = new ConditionalActionExecutor(new DbConditionalAction()
		{
			Guid = Guid.NewGuid(),
			JsBoolExpression = "${var1} === 5 && ${var2} === 10",
			ActionOnTrue = trueGuid,
			ActionOnFalse = falseGuid,
		});

		var result = await executor.Execute(context);
		Assert.NotNull(result);
		Assert.False(executor.Result.IsError);
		Assert.Equal(trueGuid, executor.Result.Next);

		executor = new ConditionalActionExecutor(new DbConditionalAction()
		{
			Guid = Guid.NewGuid(),
			JsBoolExpression = "${var1} > 5 || ${var2} > 10",
			ActionOnTrue = trueGuid,
			ActionOnFalse = falseGuid,
		});

		result = await executor.Execute(context);
		Assert.NotNull(result);
		Assert.False(executor.Result.IsError);
		Assert.Equal(falseGuid, executor.Result.Next);

		executor = new ConditionalActionExecutor(new DbConditionalAction()
		{
			Guid = Guid.NewGuid(),
			JsBoolExpression = "abracadabra!",
			ActionOnTrue = trueGuid,
			ActionOnFalse = falseGuid,
		});

		await Assert.ThrowsAsync<Esprima.ParserException>(async () => await executor.Execute(context));
	}

	[Fact]
	public async Task SimpleScenario()
	{
		// TODO: impl
	}
}
