using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wtt_main_server_data.Database.TestScenarios;
using wtt_main_server_data.Structures;

namespace wtt_main_server_data.Api;

public class ScenarioWithActions
{
	[Required]
	public DbTestScenario Scenario { get; set; } = null!;

	[Required]
	public ActionsCollection Actions { get; set; } = null!;
}
