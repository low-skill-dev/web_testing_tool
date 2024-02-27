﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wtt_main_server_data.Enums;
using wtt_main_server_data.Database.Common;

namespace wtt_main_server_data.Api;
public class UserPublicInfo
{
	public UserRoles Role { get; set; }
	public DateTime RegistrationDate { get; set; }
	public DateTime PasswordLastChanged { get; set; }

	public string Email { get; set; }
	public DateTime? EmailConfirmedAtUtc { get; set; }



	public UserPublicInfo(DbUser source)
	{
		this.Role = source.Role;
		this.RegistrationDate = source.RegistrationDate;
		this.Email = source.Email;
		this.EmailConfirmedAtUtc = source.EmailConfirmedAtUtc;
		PasswordLastChanged = source.PasswordLastChanged;
	}
}
