using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;
using Models.Database.Abstract;
using System.Net;
using Models.Enums;
using CommonLibrary.Models;
using CommonLibrary.Models;

namespace Models.Database.Common;

public class DbUser : ObjectWithGuid
{
	#region basic props

	public UserRoles Role { get; set; } = UserRoles.Regular;
	public bool IsDisabled { get; set; } = false;

	public DateTime Created { get; set; }
	public DateTime Changed { get; set; }

	public IPAddress RegistrationIPAddress { get; set; } = null!;
	public string? RegistrationCountry { get; set; } = null;
	public string? RegistrationCity { get; set; } = null;

	//	use refresh tokens data
	//	[DefaultValue(0)]
	//	public DateTime LastAccessDate = DateTime.UtcNow;

	#endregion

	#region email

	[NotNull]
	[MaxLength(64)]
	[DefaultValue("")]
	public string Email { get; set; } = string.Empty;

	public DateTime EmailConfirmedAtUtc { get; set; } = DateTime.UnixEpoch;

	#endregion

	#region password

	[MaxLength(512 / 8)]
	public byte[] PasswordSalt { get; set; } = null!;

	[MaxLength(512 / 8)]
	public byte[] PasswordHash { get; set; } = null!;

	[DefaultValue(0)]
	public DateTime PasswordLastChanged { get; set; } = DateTime.UtcNow;

	[MaxLength(512 / 8)]
	public byte[]? TotpSecretKeyForSha512 { get; set; }

	#endregion
}
