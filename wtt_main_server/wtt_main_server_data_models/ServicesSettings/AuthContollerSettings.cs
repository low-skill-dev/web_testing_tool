using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wtt_main_server_data.ServicesSettings;

public class AuthControllerSettings
{
	public int AccessTokenLifespanSeconds { get; set; } = 60 * 5; // 5 minutes
	public int RefreshTokenLifespanSeconds { get; set; } = 60 * (60 * 24 * 180); // 180 days
	public int RecoveryTokenLifespanSeconds { get; set; } = 60 * (60 * 24 * 7); // 7 days
	public string RecoveryEmailSendFromAddress { get; set; } = null!;
	public string RecoveryEndpointWithoutToken { get; set; } = null!;
}
