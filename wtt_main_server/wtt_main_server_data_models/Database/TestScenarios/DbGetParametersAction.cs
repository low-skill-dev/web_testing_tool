using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wtt_main_server_data.Database.Abstract;

namespace wtt_main_server_data.Database.TestScenarios;

/// <summary>
/// Действие, которое загружает параметры в текущий контекст,
/// выполняя GET-запрос к удаленному серверу. Удаленный сервер
/// должен вернуть ответ в виде массива соответствий ключ-значение,
/// где ключ - имя присваиваемой переменной,
/// значение, соответственно - значение.
/// </summary>
public class DbGetParametersAction : ADbWebRequest
{
	public override string Type => "GetParameters";
}
