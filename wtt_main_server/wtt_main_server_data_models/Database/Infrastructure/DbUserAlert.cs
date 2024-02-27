using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using webooster.DataModels;

namespace wtt_main_server_data.Database.Infrastructure;

#pragma warning disable CS8618

public class DbUserAlert : ADbObjectWithRelatedUser
{
	public string? Subject { get; set; }
	public string Message { get; set; }
	public int ArgbColor { get; set; }

	public bool WasConfirmedByUser { get; set; } = false;
	public bool WasSendByEmail { get; set; } = false;
}
