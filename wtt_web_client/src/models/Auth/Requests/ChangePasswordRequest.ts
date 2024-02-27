export default class ChangePasswordRequest
{
	Password: string;

	constructor(password: string)
	{
		this.Password = password;
	}
}