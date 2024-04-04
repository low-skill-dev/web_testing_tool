using Reinforced.Typings.Attributes;

namespace CommonLibrary.Models;

//[TsClass(AutoExportMethods = false,Order =100)]
public class ObjectWithUser : ObjectWithGuid
{
	public Guid? UserGuid { get; set; }
}
