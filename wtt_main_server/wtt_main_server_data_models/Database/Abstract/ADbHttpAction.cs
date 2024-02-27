using wtt_main_server_data.Enums;
using HttpRequestMethod = wtt_main_server_data.Enums.HttpRequestMethod;

namespace wtt_main_server_data.Database.Abstract;

#pragma warning disable CS8618

public abstract class ADbHttpAction : ADbWebRequest
{
	/* Метод запроса к удаленному серверу.
	 * Основной набор: GET/POST/PUT/PATCH/DELETE
	 */
	public HttpRequestMethod Method { get; set; }
	
	public HttpTlsValidationMode TlsValidationMode { get; set; }
}
