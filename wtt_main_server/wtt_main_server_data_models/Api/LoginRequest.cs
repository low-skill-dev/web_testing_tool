﻿using System.Text.Json.Serialization;

namespace wtt_main_server_data.Api;

#pragma warning disable CS8618

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
