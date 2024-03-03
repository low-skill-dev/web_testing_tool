using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wtt_main_server_data.ServicesSettings;

public class WttJwtServiceSettings
{
	public int AccessTokenLifespanSeconds { get; set; } = 60 * 5; // 5 minutes
	public int RefreshTokenLifespanSeconds { get; set; } = 60 * (60 * 24 * 180); // 180 days
	public int RecoveryTokenLifespanSeconds { get; set; } = 60 * (60 * 24 * 7); // 7 days

	public string? RecoveryEmailSendFromAddress { get; set; } // i.e. 'no-reply@vdb.lowskill.dev'
	public string? RecoveryEndpointPath { get; set; } // i.e. '/api/v1/auth/recovery'
}
