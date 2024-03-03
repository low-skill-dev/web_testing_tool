using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wtt_main_server_data.Application.TestScenarios.Parameter;
using Xunit.Abstractions;
using static wtt_main_server_data.Enums.TestScenarioArgTypes;
using wtt_main_server_scenario_executor.Helpers;

namespace wtt_main_server_tests.wtt_main_server_scenario_executor.Tests;

public class ScenarioExecutionHelperTests : webooster.Abstract.AXUnitTests
{
    public ScenarioExecutionHelperTests(ITestOutputHelper o) : base(o)
    {

    }

    [Fact]
    public void CanMergeContexts()
    {
        var dict1 = new Dictionary<string, ScenarioArgument>()
        {
            { "var1", new() { Type = Numeric, Value = "11" } },
            { "var3", new() { Type = Numeric, Value = "31" } },
        };

        var dict2 = new Dictionary<string, ScenarioArgument>()
        {
            { "var1", new() { Type = Numeric, Value = "12" } },
            { "var2", new() { Type = Numeric, Value = "21" } },
        };

        var merged = ContextHelper.MergeContexts(dict1, dict2);

        Assert.Equal(3, merged.Count);
        Assert.True(merged.Keys.SequenceEqual(new string[] { "var1", "var3", "var2" }));
		Assert.True(merged.Values.Select(x => x.Value).SequenceEqual(new string[] { "12", "31", "21" }));
    }

    [Fact]
    public void CanInterpolateValues()
    {
        var dict = new Dictionary<string, string>()
        {
            { "var1", "11" },
            { "var2",   "22" },
            { "var3",   "33" },
        };

        var var1 = dict["var1"];
        var var2 = dict["var2"];
        var var3 = dict["var3"];

        var common = $"This message 11 will be send to 22 in 33.";

        var t = "This message ${var1} will be send to ${var2} in ${var3}.";
        var smart = ContextHelper.CreateStringFromContext(t, dict);

        Assert.Equal(common, smart);
    }

    [Fact]
    public void CanIgnoreValuesThatShouldNotBeInterpolated()
    {
        var dict = new Dictionary<string, string>()
        {
            { "var1","11"  },
            { "var2","22"  },
            { "var3", "33"  },
        };

        var var1 = dict["var1"];
        var var2 = dict["var2"];
        var var3 = dict["var3"];

        var common = "This message {var1} will be send to {var2} in {var3}.";

        var t = "This message {var1} will be send to {var2} in {var3}.";
        var smart = ContextHelper.CreateStringFromContext(t, dict);

        Assert.Equal(common, smart);
    }
}