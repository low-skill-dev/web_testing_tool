using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Enums;

public enum HttpTlsValidationMode
{
    Enabled, // >= TLS 1.2
    AllowSelfSigned, // >= TLS 1.2
    Disabled, // no-tls + self-signed + old versions
}
