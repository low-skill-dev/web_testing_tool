using DbConcept;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wtt_main_server_data.Enums;

namespace wtt_main_server_data.Database.Networking;

#pragma warning disable CS8618

public class DbUserProxy : ObjectWithUser
{
	public string Name { get; set; }
	public string Username { get; set; }
	public string Password { get; set; }
	public ProxyTypes Type { get; set; }
}
