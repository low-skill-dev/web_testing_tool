using Models.Enums;
using CommonLibrary.Models;

namespace Models.Database.Networking;

#pragma warning disable CS8618

public class DbUserProxy : ObjectWithUser
{
	public string Name { get; set; }
	public string Username { get; set; }
	public string Password { get; set; }
	public ProxyTypes Type { get; set; }
}
