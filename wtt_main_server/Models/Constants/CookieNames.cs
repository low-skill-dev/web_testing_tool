using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Constants;

public class CookieNames
{
	public const string 
		JwtAccessTokenCookieName = @"access_jwt",
		JwtRefreshTokenCookieName = @"refresh_jwt";
}
