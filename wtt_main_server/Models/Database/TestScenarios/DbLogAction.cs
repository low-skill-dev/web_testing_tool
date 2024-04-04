using Models.Database.Abstract;
using Models.Enums;
using Reinforced.Typings.Attributes;

namespace Models.Database.TestScenarios;

//[TsClass(IncludeNamespace = false, Order = 500)]
public class DbLogAction : ADbAction
{
	public override ActionTypes Type { get; set; } = ActionTypes.DbErrorActionType;

	public string Message { get; set; } = "Error";
	public bool StopExecution { get; set; } = true;
}
