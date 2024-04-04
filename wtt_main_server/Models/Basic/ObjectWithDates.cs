using Reinforced.Typings.Attributes;

namespace CommonLibrary.Models;

//[TsClass(AutoExportMethods = false, Order = 80)]
public class ObjectWithDates
{
	public DateTime Created { get; set; }
	public DateTime? Changed { get; set; }
}