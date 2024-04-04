using Reinforced.Typings.Attributes;
using System.Text.Json.Serialization;

namespace Models.Api;

#pragma warning disable CS8618

//[TsClass(AutoExportMethods = false, Order = 300)]
public class LoginRequest : RegistrationRequest
{
	public string? TotpCode { get; set; }

	[JsonConstructor] public LoginRequest() { }

	public LoginRequest(RegistrationRequest rr)
	{
		this.Email = rr.Email;
		this.Password = rr.Password;
	}
}
