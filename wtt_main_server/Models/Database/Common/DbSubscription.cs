using CommonLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Database.Common;

public class DbSubscription : ObjectWithUser
{
	public Guid TariffGuid { get; set; }
	public DateTime Starts { get; set; }
	public DateTime Expires { get; set; }
}
