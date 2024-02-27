using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wtt_main_server_data.Constants;

public static class ClaimNames
{
	public const string
		JwtFromCookieClaimPrincipalName = nameof(JwtFromCookieClaimPrincipalName),
		Custom = nameof(Custom);
}
