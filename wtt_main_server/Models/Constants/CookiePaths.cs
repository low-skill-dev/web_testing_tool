using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Constants;

public class CookiePaths
{
	public const string 
		JwtRefreshTokenCookiePath = @"/api/auth",
		JwtAccessTokenCookiePath = @"/";
}
