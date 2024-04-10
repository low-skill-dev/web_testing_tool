using Models.Database.Abstract;
using Models.Enums;
using Reinforced.Typings.Attributes;
using HttpMethod = Models.Enums.HttpRequestMethod;

namespace Models.Database.TestScenarios;

#pragma warning disable CS8618

//[TsClass(IncludeNamespace = false, Order = 500)]
public class DbHttpAction : ADbHttpAction
{
	public override ActionTypes Type { get; set; } = ActionTypes.DbHttpActionType;

	/* Тело, заголовки, куки. В видео форматироуемых строк, в которые будут
	 * вставлены переменные из текущего контекста выполнения операции.
	 * Для заголовков и кук соответственно - имя к значению.
	 */
	public string? RequestBody { get; set; }
	public Dictionary<string, string>? RequestHeaders { get; set; }
	public Dictionary<string, string>? RequestCookies { get; set; }


	/* Действие при неспособности обработать запрос без прокси.
	 * По началу будем всем задавать ThrowAsNormally - потом
	 * допишем код который уже будет думать, находить ближайшую 
	 * страну.
	 * 
	 * Овер инжиниринг? Пока закомментил. Пусть делают обработчик
	 * через иф.
	 */
	//public OnProxyErrorActions? OnProxyErrorAction { get; set; }


	/* Полноценный юзер-скрипт, который будет выполнен над ответом
	 * от сервера. Тут можно будет написать любой JS-код.
	 * В нём можно будет использовать переменные из контекста
	 * сценария. Не очень очевидно, стоит ли обновлять переменную в
	 * контексте просто по факту её изменения, но в остальном, этот
	 * код может вернуть словарь ИМЯ-ЗНАЧЕНИЕ для обновления переменных
	 * в контексте сценария.
	 * 
	 * Для доступности переменных в самом коде, программа должна
	 * сгенерировать объявление и присвоение всех переменных перед
	 * юзер скриптом.
	 */
	//public string? UserScript { get; set; } // Скрипт вытаскивания переменных. Должен возвращать словарь.
	//public bool UseTryBlockForUserScript { get; set; } = false;

	/* Другой вариант работы. Если полноценный юзер-скрипт предназначен
	 * для опытнах пользователей, то данный вариант позволяет упростить
	 * и ускорить работу для наиболе базового сценария - парсинга ответа
	 * в формате JSON. В данном случае пользователь просто указывает путь
	 * в JSON-ответе, по которому будет получен доступ к значению, далее
	 * указывает, в какую переменную его записать.
	 * 
	 * VariableNamesToUpdate - имена переменных в контексте сценария,
	 * которые будут обновлены.
	 * 
	 * VariablePaths - путь к значениям, которые нужно записать в
	 * соответствующие переменные.
	 */
	//Dictionary<string,string> BodyVariablesToExtract { get; set; }
	//Dictionary<string,string> CookiesVariablesToExtract { get; set; }
	//Dictionary<string, string> HeadersVariablesToExtract { get; set; }


	///* В какие переменные будет целиком записана та или иная часть ответа.
	// */
	//public string? WriteBodyStringToVariable { get; set; }
	//public string? WriteResponseHeadersDictionaryToVariable { get; set; }
	//public string? WriteResponseCookiesDictionaryToVariable { get; set; }


	//public string? WriteContentLengthToVariable { get; set; }

	//public string? WriteStatusCodeToVariable { get; set; }
	//public string? WriteResponseMimeTypeToVariable { get; set; }
	//public string? WriteHttpVersionToVariable { get; set; }

	/// <summary>
	/// Variables, updates of which in the generated JS code
	/// should be placed into the try/catch/finally block.
	/// </summary>
	public List<string>? VariablesUpdatedInTryBlock { get; set; }
}
