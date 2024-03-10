using Models.Database.Abstract;

namespace Models.Database.TestScenarios;

public class DbErrorAction : ADbAction
{
	public override string Type => "Error";

	public string Message { get; set; } = "Error";
	public bool IsCritical { get; set; } = true;
}
