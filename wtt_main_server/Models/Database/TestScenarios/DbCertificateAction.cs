using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Database.Abstract;
using Models.Enums;
using Reinforced.Typings.Attributes;

namespace Models.Database.TestScenarios;

// Запрос сертификата по Url
//[TsClass(IncludeNamespace = false, Order = 500)]
public class DbCertificateAction : ADbHttpAction
{
	public override ActionTypes Type { get; set; } = ActionTypes.DbCertificateActionType;
}
