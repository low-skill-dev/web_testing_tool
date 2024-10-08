﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScenarioExecutor.Helpers;
public static class ContextHelper
{
	/// <summary>
	/// Возвращает новый словарь, представляющий слияние "предыдущего" и "следующего"
	/// контекстов, где содержатся все ключи - из обоих контекстов, но для ключей,
	/// содержащихся в обоих словарях устанавливается значение из "следующего" контекста.
	/// Оригинальные коллекции не модифицируются.
	/// </summary>
	public static Dictionary<string, string> MergeContexts(
		IDictionary<string, string> prev, IDictionary<string, string> next)
	{
		var ret = new Dictionary<string, string>(prev.Keys.Union(next.Keys).Count());

		foreach(var pair in prev)
		{
			ret.Add(pair.Key, pair.Value);
		}

		foreach(var pair in next)
		{
			if(ret.ContainsKey(pair.Key))
				ret[pair.Key] = pair.Value;
			else
				ret.Add(pair.Key, pair.Value);
		}

		return ret;
	}

	/// <summary>
	/// Вставляет в строку переменные по паттерну ${varName}, игнорируя {varName}.
	/// </summary>
	public static string CreateStringFromContext(string input, IDictionary<string, string> context)
	{
		StringBuilder sb = new(input.Length + context.Values.Sum(x => x.Length));

		for(int i = 0; i < input.Length; i++)
		{
			if(input[i] != '$' || i == input.Length - 1)
			{
				sb.Append(input[i]);
				continue;
			}

			if(input[i + 1] != '{')
			{
				continue;
			}

			var closing = FindClosing(input, i + 1);
			var varName = input.Substring(i + 2, closing - (i + 2));
			sb.Append(context[varName]);
			i = closing;
		}

		return sb.ToString();

		static int FindClosing(string input, int open)
		{
			if(input[open] != '{') throw new ArgumentException(nameof(open));

			for(int i = open; i < input.Length; i++) if(input[i] == '}') return i;

			throw new ArgumentException(nameof(input));
		}
	}
}