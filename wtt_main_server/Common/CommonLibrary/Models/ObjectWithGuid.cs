using System.ComponentModel.DataAnnotations;

namespace CommonLibrary.Models;

public class ObjectWithGuid : ObjectWithDates
{
	[Key]
	public Guid Guid { get; set; }
}
