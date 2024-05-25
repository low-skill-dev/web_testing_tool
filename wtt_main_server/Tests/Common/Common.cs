using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Common;
public class Common
{
    [Fact]
    public async Task CanOverrideStopwatchTicks()
    {
        var source = new Stopwatch();

        source.Start();
        await Task.Delay(System.Security.Cryptography.RandomNumberGenerator.GetInt32(8, 2048));
        source.Stop();

        Assert.True(source.ElapsedTicks > 10);

        var overridem = new Stopwatch();
        Assert.Equal(0, overridem.ElapsedTicks);

        overridem.GetType().GetField("_elapsed",
            System.Reflection.BindingFlags.NonPublic |
            System.Reflection.BindingFlags.Instance)!
            .SetValue(overridem, source.ElapsedTicks);

        Assert.Equal(source.ElapsedTicks, overridem.ElapsedTicks);
    }
}
