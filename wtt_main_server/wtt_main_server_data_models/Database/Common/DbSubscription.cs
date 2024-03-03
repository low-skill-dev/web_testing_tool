using DbConcept;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wtt_main_server_data.Database.Common;

public class DbSubscription : ObjectWithUser
{
	public Guid TariffGuid { get; set; }
	public DateTime Starts { get; set; }
	public DateTime Expires { get; set; }
}
