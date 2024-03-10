using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Database.Abstract;
using Models.Enums;
using CommonLibrary.Models;

namespace Models.Database.Common;

#pragma warning disable CS8618

public class DbEmailSendLog : ObjectWithUser
{
	public EmailTypes Type { get; set; }
	public string Addressee { get; set; }
	public DateTime Created { get; set; }
	public bool IsSucceeded { get; set; }
}
