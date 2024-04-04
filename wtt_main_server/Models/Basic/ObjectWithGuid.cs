using Reinforced.Typings.Attributes;
using System.ComponentModel.DataAnnotations;

namespace CommonLibrary.Models;

//[TsClass(AutoExportMethods = false, Order = 90)]
public class ObjectWithGuid : ObjectWithDates
{
	[Key]
	public Guid Guid { get; set; }
}
