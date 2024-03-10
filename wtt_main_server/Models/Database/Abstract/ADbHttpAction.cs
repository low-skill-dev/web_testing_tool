using Models.Enums;
using HttpRequestMethod = Models.Enums.HttpRequestMethod;

namespace Models.Database.Abstract;

#pragma warning disable CS8618

public abstract class ADbHttpAction : ADbWebRequest
{
	/* Метод запроса к удаленному серверу.
	 * Основной набор: GET/POST/PUT/PATCH/DELETE
	 */
	public HttpRequestMethod Method { get; set; }
	
	public HttpTlsValidationMode TlsValidationMode { get; set; }
}
