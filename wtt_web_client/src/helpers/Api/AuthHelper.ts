import axios from "axios";
import Common from "../Common/Common";
import UrlHelper from "./UrlHelper";
import AuthorizedApiInteractionBase from "./AuthorizedApiInteractionBase";
import Constants from "../Common/Constants";
//import GlobalContext from "../GlobalContext";
import EnvHelper from "../Common/EnvHelper";
import { ChangePasswordRequest, IJwtResponse, LoginRequest, RegistrationRequest } from "src/csharp/project";

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
		const req = new LoginRequest();
		req.Email = login;
		req.Password = password;
		req.TotpCode = totp;
		const res = await axios.post<IJwtResponse>(UrlHelper.Backend.V1.Auth.Post.Login, req);

		if (Common.Between(200, res.status, 299)) this.setJwt(res.data.Access, res.data.Refresh)

		return res.status;
	}
	public static Register = async (login: string, password: string): Promise<number> =>
	{
		const req = new RegistrationRequest();
		req.Email = login;
		req.Password = password;
		const res = await axios.put<IJwtResponse>(UrlHelper.Backend.V1.Auth.Put.Register, req);

		if (Common.Between(200, res.status, 299)) this.setJwt(res.data.Access, res.data.Refresh)

		return res.status;
	}
	public static TerminateSession = async (jtiSha?: string): Promise<number> =>
	{
		// before request
		await AuthorizedApiInteractionBase.Create();

		let query = jtiSha ? `?jtiShaHex=${jtiSha}` : "";

		return (
			await axios.delete<IJwtResponse>(
				UrlHelper.Backend.V1.Auth.Delete.TerminateSession + query)
		).status;
	}

	public static ChangePassword = async (password: string): Promise<number> =>
	{
		// before request
		await AuthorizedApiInteractionBase.Create();

		const req = new ChangePasswordRequest();
		req.Password = password;
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
		const req = new ChangePasswordRequest();
		req.Password = password
		const res = await axios.post(UrlHelper.Backend.V1.Auth.Post.ResetPassword + `/${jwt}`, req,);

		return res.status;
	}
}