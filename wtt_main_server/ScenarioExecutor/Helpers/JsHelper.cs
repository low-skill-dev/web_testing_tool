using Esprima;
using Jint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Database.Common;

namespace ScenarioExecutor.Helpers;

internal static class JsHelper
{
	public static void Execute(Action<Engine> functionsAdding, string code, DbTariff limitations,
		bool ignoreErrors = false)
	{
		var eng = new Engine(o =>
		{
			o.LimitMemory(limitations.MaximumScriptMemoryBytes);
			o.TimeoutInterval(TimeSpan.FromSeconds(limitations.MaximumScriptTimeSecs));
		});

		functionsAdding(eng);

		eng.Execute(code, new ParserOptions { Tolerant = ignoreErrors });
	}
}
