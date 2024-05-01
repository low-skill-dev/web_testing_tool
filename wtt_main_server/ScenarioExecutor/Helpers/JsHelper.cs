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
	public static void Execute(Action<Engine> functionsAdding, string code, bool ignoreErrors = false)
	{
		var eng = new Engine(o =>
		{
			o.LimitMemory(8 * 1024 * 1024); // 8 mb
			o.TimeoutInterval(TimeSpan.FromSeconds(4));
		});

		functionsAdding(eng);

		eng.Execute(code, new ParserOptions { Tolerant = ignoreErrors });
	}
}
