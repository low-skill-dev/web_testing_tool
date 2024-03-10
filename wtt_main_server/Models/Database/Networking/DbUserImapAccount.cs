using CommonLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Database.Networking;

#pragma warning disable CS8618

public class DbUserImapAccount : ObjectWithUser
{
	public string Name { get; set; }
	public string ConnectionUrl { get; set; }
	public string Username { get; set; }
	public string Password { get; set; }
}
