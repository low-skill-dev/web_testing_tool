import HttpRequestMethod from "src/models/Enums/HttpRequestMethod";
import ADbWebRequest from "./ADbWebRequest";
import HttpTlsValidationMode from "src/models/Enums/HttpTlsValidationMode";

export default abstract class ADbHttpAction extends ADbWebRequest
{
	method: HttpRequestMethod = HttpRequestMethod.Get;
	tlsValidationMode: HttpTlsValidationMode = HttpTlsValidationMode.Enabled;
}