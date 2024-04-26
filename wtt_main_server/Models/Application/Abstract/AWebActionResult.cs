using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Application.Abstract;

#pragma warning disable CS8618

[Obsolete("Не требуется")]
public abstract class AWebActionResult : AActionResult
{
	public string Url { get; set; }

	//public AWebActionResult(Guid dbActionGuid, string url) : base(dbActionGuid)
	//{
	//	Url = url;
	//}
}
