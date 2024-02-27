using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wtt_main_server_data.Enums;

namespace wtt_main_server_data.Application.TestScenarios.Parameter;

public class ScenarioArgument
{
	public required TestScenarioArgTypes Type { get; init; }
	public required string Value { get; init; }

	public override string ToString()
	{
		return Value;
	}
}
