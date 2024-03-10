using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Enums;

public enum HttpRequestMethod
{
    Get = 1,
    Post = 2,
    Put = 4,
    Patch = 8,
    Delete = 16,
}
