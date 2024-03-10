namespace WebApi.Controllers;

public static class ResponseMessages
{
	public const string
		TotpRequired = "TOTP required",
		InvalidCredentials = "Invalid credentials",
		TotpDidNotMatch = "TOTP did not match",
		UserAlreadyExists = "User already exists",
		TooYoungSession = "Your session is too new for this operation";
}
