using Reinforced.Typings.Attributes;

namespace Models.Api;

#pragma warning disable CS8618

//[TsClass(AutoExportMethods = false, Order = 200)]
public class RegistrationRequest : ChangePasswordRequest
{
    public string Email { get; set; } = "";
}
