using TestHelper;
using Models.Database.TestScenarios;
using ScenarioExecutor.ActionExecutors;
using Xunit.Abstractions;


namespace Tests.ScenarioExecutor;


public class HttpScenarioExecutorTests : ATestsWithLogging
{
	private HttpActionExecutor _defaultExecutor;
	private Dictionary<string, string> _emptyContext;

	public HttpScenarioExecutorTests(ITestOutputHelper o) : base(o)
	{
		_defaultExecutor = new HttpActionExecutor(new());
		_emptyContext = new();
	}

	[Fact]
	public void CanCreate()
	{
		var executor = new HttpActionExecutor(new());

		Assert.NotNull(executor);
	}

	[Fact]
	public async Task CanExecuteSimple()
	{
		var url = @"https://jsonplaceholder.typicode.com/posts/1";

		var action = new DbHttpAction()
		{
			Guid = Guid.NewGuid(),
			Method = Models.Enums.HttpRequestMethod.Get,
			RequestUrl = url,
		};


		var executor = new HttpActionExecutor(action);

		var result = await executor.Execute(_emptyContext);

		Assert.NotNull(result);
	}
}
