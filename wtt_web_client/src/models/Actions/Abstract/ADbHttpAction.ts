import HttpRequestMethod from "src/models/Enums/HttpRequestMethod";
import ADbWebRequest from "./ADbWebRequest";
import HttpTlsValidationMode from "src/models/Enums/HttpTlsValidationMode";

export default interface ADbHttpAction extends ADbWebRequest
{
	method: HttpRequestMethod;
	tlsValidationMode: HttpTlsValidationMode;
}