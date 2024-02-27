namespace wtt_main_server_api.Controllers;

public static class ResponseMessages
{
	public const string
		TotpRequired = "TOTP required",
		InvalidCredentials = "Invalid credentials",
		TotpDidNotMatch = "TOTP did not match",
		UserAlreadyExists = "User already exists";
}
