namespace Models.Api;

#pragma warning disable CS8618

public class RegistrationRequest : ChangePasswordRequest
{
    public string Email { get; set; }
}
