using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Database.TestScenarios;
using Models.Structures;

namespace Models.Api;

public class ScenarioWithActions
{
	[Required]
	public DbTestScenario Scenario { get; set; } = null!;

	[Required]
	public ActionsCollection Actions { get; set; } = null!;
}
