import ChangePasswordRequest from "./ChangePasswordRequest";

export default class RegistrationRequest extends ChangePasswordRequest
{
	Email: string;

	constructor(email: string, password: string)
	{
		super(password);
		this.Email = email;
	}
}