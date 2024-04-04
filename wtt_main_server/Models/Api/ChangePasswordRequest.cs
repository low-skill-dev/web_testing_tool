using Reinforced.Typings.Attributes;

namespace Models.Api;

#pragma warning disable CS8618

//[TsClass(AutoExportMethods = false, Order = 100)]
public class ChangePasswordRequest
{
	public string Password { get; set; } = "";
}
