using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using webooster.Abstract;
using wtt_main_server_data.Database.Common;
using wtt_main_server_data.Database.TestScenarios;
using wtt_main_server_scenario_executor.ActionExecutors;
using Xunit.Abstractions;

namespace wtt_main_server_tests.wtt_main_server_scenario_executor.Tests;

public class HttpScenarioExecutorTests : AXUnitTests
{
	private HttpActionExecutor _defaultExecutor;
	private Dictionary<string, string> _emptyContext;

	public HttpScenarioExecutorTests(ITestOutputHelper o) : base(o)
	{
		_defaultExecutor = new HttpActionExecutor(new()) { Tariff = new() };
		_emptyContext = new();
	}

	[Fact]
	public void CanCreate()
	{
		var executor = new HttpActionExecutor(new()) { Tariff = new() };

		Assert.NotNull(executor);
	}

	[Fact]
	public async Task CanExecuteSimple()
	{
		var url = @"https://jsonplaceholder.typicode.com/posts/1";

		var action = new DbHttpAction()
		{
			Id = 1,
			Guid = Guid.NewGuid(),
			Method = wtt_main_server_data.Enums.HttpRequestMethod.Get,
			RequestUrl = url,
		};


		var executor = new HttpActionExecutor(action) { Tariff = new() };

		var result = await executor.Execute(_emptyContext);

		Assert.NotNull(result);
	}
}
