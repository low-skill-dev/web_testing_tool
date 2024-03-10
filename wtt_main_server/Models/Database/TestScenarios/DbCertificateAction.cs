using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Database.Abstract;

namespace Models.Database.TestScenarios;

// Запрос сертификата по Url
public class DbCertificateAction : ADbHttpAction
{
	public override string Type => "Certificate";
}
