using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScenarioExecutor.Helpers;

public static class StringExtensions
{
	public static bool IsNullOrWhiteSpaceAny(params string[] v)
		=> v.Any(x => string.IsNullOrWhiteSpace(x));
}
