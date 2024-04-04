using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Database.Abstract;
using Models.Enums;
using Reinforced.Typings.Attributes;

namespace Models.Database.TestScenarios;

/// <summary>
/// Действие, которое загружает параметры в текущий контекст,
/// выполняя GET-запрос к удаленному серверу. Удаленный сервер
/// должен вернуть ответ в виде массива соответствий ключ-значение,
/// где ключ - имя присваиваемой переменной,
/// значение, соответственно - значение.
/// </summary>
//[TsClass(IncludeNamespace = false, Order = 500)]
public class DbGetParametersAction : ADbWebRequest
{
	public override ActionTypes Type { get; set; } = ActionTypes.DbGetParametersActionType;
}
