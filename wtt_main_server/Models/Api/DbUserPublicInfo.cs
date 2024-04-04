using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Enums;
using Models.Database.Common;
using CommonLibrary.Models;
using Reinforced.Typings.Attributes;

namespace Models.Api;

//[TsInterface(AutoExportMethods = false)]
public class DbUserPublicInfo : ObjectWithGuid
{
	public UserRoles Role { get; }
	public bool IsDisabled { get; }

	public DateTime RegistrationDate { get; }

	public string Email { get; }
	public DateTime? EmailConfirmedAtUtc { get; }
	public bool IsEmailConfirmed => EmailConfirmedAtUtc.HasValue;

	public DateTime PasswordLastChanged { get; }


	public DbUserPublicInfo(DbUser source)
	{
		Guid = source.Guid;
		Role = source.Role;
		IsDisabled = source.IsDisabled;
		Created = source.Created;
		Changed = source.Changed;
		Email = source.Email;
		EmailConfirmedAtUtc = source.EmailConfirmedAtUtc;
		PasswordLastChanged = source.PasswordLastChanged;
	}
}
