using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using wtt_main_server_data.Application.Abstract;

namespace wtt_main_server_data.Application.TestScenarios.ActionResults;
public class CertificateActionResult : AWebActionResult
{
	public X509Certificate? RetrievedCertificate { get; set; }
}
