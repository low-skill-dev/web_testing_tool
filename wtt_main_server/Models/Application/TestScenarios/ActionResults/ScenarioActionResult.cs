using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Application.Abstract;

namespace Models.Application.TestScenarios.ActionResults;
public class ScenarioActionResult : AActionResult
{
	public Guid CreatedTaskGuid { get; set; }
}
