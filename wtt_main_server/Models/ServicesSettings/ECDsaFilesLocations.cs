using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ServicesSettings;
public class ECDsaFilesLocations
{
	public string Jwt { get; set; } = "/etc/wtt/ecdsa/jwt.key";
}
