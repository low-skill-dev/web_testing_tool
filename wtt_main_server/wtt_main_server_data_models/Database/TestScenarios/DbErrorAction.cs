using wtt_main_server_data.Database.Abstract;

namespace wtt_main_server_data.Database.TestScenarios;

public class DbErrorAction : ADbAction
{
	public override string Type => "Error";

	public string Message { get; set; } = "Error";
	public bool IsCritical { get; set; } = true;
}
