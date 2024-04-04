using Models.Enums;
using Reinforced.Typings.Attributes;
using HttpRequestMethod = Models.Enums.HttpRequestMethod;

namespace Models.Database.Abstract;

#pragma warning disable CS8618

//[TsClass(IncludeNamespace = false, Order = 400)]
public abstract class ADbHttpAction : ADbWebRequest
{
	/* Метод запроса к удаленному серверу.
	 * Основной набор: GET/POST/PUT/PATCH/DELETE
	 */
	public HttpRequestMethod Method { get; set; }

	public HttpTlsValidationMode TlsValidationMode { get; set; } = 0;
}
