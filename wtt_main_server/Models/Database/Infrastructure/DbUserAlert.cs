using CommonLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Database.Infrastructure;

#pragma warning disable CS8618

public class DbUserAlert : ObjectWithUser
{
	public string? Subject { get; set; }
	public string Message { get; set; }
	public int ArgbColor { get; set; }

	public bool WasConfirmedByUser { get; set; } = false;
	public bool WasSendByEmail { get; set; } = false;
}
