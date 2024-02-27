using System;
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

public class DbUserPublicInfo
{
    public Guid Guid { get; }
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
        RegistrationDate = source.RegistrationDate;
        Email = source.Email;
        EmailConfirmedAtUtc = source.EmailConfirmedAtUtc;
        PasswordLastChanged = source.PasswordLastChanged;
    }
}
