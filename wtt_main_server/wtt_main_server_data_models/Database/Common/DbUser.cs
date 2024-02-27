using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;
using wtt_main_server_data.Database.Abstract;
using System.Net;
using webooster.DataModels;
using wtt_main_server_data.Enums;

namespace wtt_main_server_data.Database.Common;

public class DbUser : ADbObjectWithGuid
{
	#region basic props

	[DefaultValue(0)]
	public UserRoles Role { get; set; } = 0;

	[DefaultValue(false)]
	public bool IsDisabled { get; set; } = false;

	[DefaultValue(0)]
	public DateTime RegistrationDate = DateTime.UtcNow;

	//	use refresh tokens data
	//	[DefaultValue(0)]
	//	public DateTime LastAccessDate = DateTime.UtcNow;

	#endregion

	#region email

	[NotNull]
	[MaxLength(64)]
	[DefaultValue("")]
	public string Email { get; set; } = string.Empty;

	[DefaultValue(null)]
	public DateTime? EmailConfirmedAtUtc { get; set; } = null;

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
