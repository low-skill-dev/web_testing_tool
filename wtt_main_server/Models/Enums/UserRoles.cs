﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Enums;

[Flags]
public enum UserRoles : int
{
    Regular = 128,
    Moderator = 32768,
    Administrator = 4194304,
    CEO = 536870912,
}
