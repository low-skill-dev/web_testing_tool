using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Enums;

namespace Models.Application.TestScenarios.Parameter;

[Obsolete("Отказ от разработки по причине нехватки времени")]
public class ScenarioArgument
{
	public required TestScenarioArgTypes Type { get; init; }
	public required string Value { get; init; }

	public override string ToString()
	{
		return Value;
	}
}
