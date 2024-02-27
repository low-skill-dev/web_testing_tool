using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wtt_main_server_data.Database.Abstract;

namespace wtt_main_server_data.Database.TestScenarios;

// Запрос сертификата по Url
public class DbCertificateAction : ADbHttpAction
{
	public override string Type => "Certificate";
}
