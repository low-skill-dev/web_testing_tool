using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Enums;

namespace Models.Database.TestExecutors;

public class HttpClientSettings
{
	public HttpTlsValidationMode TlsValidationMode { get; init; }
}
