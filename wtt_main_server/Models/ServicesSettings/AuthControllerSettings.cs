using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ServicesSettings;

public class AuthControllerSettings
{
	public string RecoveryEndpointWithoutToken { get; set; } = @"https://wtt.lowskill.dev/reset-password";
}
