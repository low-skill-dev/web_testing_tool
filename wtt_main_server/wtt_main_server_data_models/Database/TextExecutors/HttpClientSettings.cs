using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wtt_main_server_data.Enums;

namespace wtt_main_server_data.Database.TestExecutors;

public class HttpClientSettings
{
	public HttpTlsValidationMode TlsValidationMode { get; init; }
}
