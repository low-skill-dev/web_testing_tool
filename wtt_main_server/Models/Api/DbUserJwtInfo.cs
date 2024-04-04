using CommonLibrary.Models;
using Models.Enums;
using Reinforced.Typings.Attributes;

namespace Models.Api;

//[TsInterface(AutoExportMethods = false,Order =128)]
public class DbUserJwtInfo : ObjectWithGuid
{
	public required string Email { get; init; }
	public required UserRoles Role { get; init; }
	public required bool IsDisabled { get; init; }
	public required DateTime EmailConfirmedAtUtc { get; init; }
	public required DateTime PasswordLastChanged { get; init; }

}
