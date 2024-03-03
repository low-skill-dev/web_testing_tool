using DbConcept;
using wtt_main_server_data.Enums;

namespace wtt_main_server_data.Api;

public class DbUserJwtInfo : ObjectWithGuid
{
	public required string Email { get; init; }
	public required UserRoles Role { get; init; }
	public required bool IsDisabled { get; init; }
	public required DateTime RegistrationDate { get; init; }
	public required DateTime EmailConfirmedAtUtc { get; init; }
	public required DateTime PasswordLastChanged { get; init; }

}
