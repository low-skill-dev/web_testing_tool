using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Constants;

public class CookiePaths
{
	public const string
		JwtAccessTokenCookiePath = @"/",
		JwtRefreshTokenCookiePathV1 = @"/api/v1/auth",
		JwtRefreshTokenCookiePathV2 = @"/api/v2/auth";
}
