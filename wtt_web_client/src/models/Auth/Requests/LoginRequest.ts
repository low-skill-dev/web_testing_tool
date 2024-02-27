import RegistrationRequest from "./RegistrationRequest";

export default class LoginRequest extends RegistrationRequest
{
	totpCode: string | null;

	constructor(login: string, password: string, totpCode: string | null)
	{
		super(login, password);
		this.totpCode = totpCode;
	}
}