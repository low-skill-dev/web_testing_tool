import axios from "axios";
import ChangePasswordRequest from "src/models/Auth/Requests/ChangePasswordRequest";
import Common from "../Common/Common";
import UrlHelper from "./UrlHelper";
import LoginRequest from "src/models/Auth/Requests/LoginRequest";
import AuthorizedApiInteractionBase from "./AuthorizedApiInteractionBase";
import JwtResponse from "src/models/Auth/Responses/JwtResponse";
import RegistrationRequest from '../../models/Auth/Requests/RegistrationRequest';
import Constants from "../Common/Constants";
import GlobalContext from "../GlobalContext";
import EnvHelper from "../Common/EnvHelper";


export default class AuthHelper
{
	private static setJwt(access: string, refresh: string)
	{
		console.log("Settings new JWTs pair.");
		if (EnvHelper.isDebugMode)
		{
			console.log(access);
			console.log(refresh);
		}
		localStorage.setItem(Constants.AccessTokenName, access);
		localStorage.setItem(Constants.RefreshTokenName, refresh);
	}

	public static Login = async (login: string, password: string, totp: string): Promise<number> =>
	{
		const req = new LoginRequest(login, password, totp);
		const res = await axios.post<JwtResponse>(UrlHelper.Backend.V1.Auth.Post.Login, req);

		if (Common.Between(200, res.status, 299)) this.setJwt(res.data.access, res.data.refresh)

		return res.status;
	}
	public static Register = async (login: string, password: string): Promise<number> =>
	{
		const req = new RegistrationRequest(login, password);
		const res = await axios.put<JwtResponse>(UrlHelper.Backend.V1.Auth.Put.Register, req);

		if (Common.Between(200, res.status, 299)) this.setJwt(res.data.access, res.data.refresh)

		return res.status;
	}
	public static TerminateSession = async (jtiSha?: string): Promise<number> =>
	{
		// before request
		await AuthorizedApiInteractionBase.Create();

		let query = jtiSha ? `?jtiShaHex=${jtiSha}` : "";

		return (
			await axios.delete<JwtResponse>(
				UrlHelper.Backend.V1.Auth.Delete.TerminateSession + query)
		).status;
	}

	public static ChangePassword = async (password: string): Promise<number> =>
	{
		// before request
		await AuthorizedApiInteractionBase.Create();

		const req = new ChangePasswordRequest(password);
		const res = await axios.patch(UrlHelper.Backend.V1.Auth.Patch.ChangePassword, req);

		return res.status;
	}
	public static ForgotPassword = async (email: string): Promise<number> =>
	{
		const res = await axios.post(UrlHelper.Backend.V1.Auth.Put.CreateAndSendLink + `/${email}`);
		return res.status;
	}
	public static ResetPassword = async (password: string, jwt: string): Promise<number> =>
	{
		const req = new ChangePasswordRequest(password);
		const res = await axios.post(UrlHelper.Backend.V1.Auth.Post.ResetPassword + `/${jwt}`, req,);

		return res.status;
	}
}