using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using webooster.DataModels;
using wtt_main_server_data.Database.Abstract;
using wtt_main_server_data.Enums;

namespace wtt_main_server_data.Database.Common;

#pragma warning disable CS8618

public class DbEmailSendLog : ADbObjectWithRelatedUser
{
	public EmailTypes Type { get; set; }
	public string Addressee { get; set; }
	public DateTime Created { get; set; }
	public bool IsSucceeded { get; set; }
}
