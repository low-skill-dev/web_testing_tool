import jwtDecode from "jwt-decode";
import IDbUserPublicInfo from "src/models/Auth/Data/IDbUserPublicInfo";
import JwtHelper from "../Common/jwtHelper";
import IJwtInfo from '../../models/Common/IJwtInfo';
import endpoints from "../../config/endpoints.json";
import UrlHelper from "./UrlHelper";
import axios from "axios";
import JwtResponse from "src/models/Auth/Responses/JwtResponse";
import Common from "../Common/Common";
import Constants from "../Common/Constants";
import EnvHelper from "../Common/EnvHelper";

export default class AuthorizedApiInteractionBase
{
	private _Access: IDbUserPublicInfo;
	private _Refresh: IJwtInfo;
	private _AccessExpires: Date;
	private _RefreshExpires: Date;

	public get Access() { return this._Access; }
	public get Refresh() { return this._Refresh; }
	public get AccessExpires() { return this._AccessExpires; }
	public get RefreshExpires() { return this._RefreshExpires; }

	private constructor(
		Access: IDbUserPublicInfo, Refresh: IJwtInfo,
		AccessExpires: Date, RefreshExpires: Date)
	{
		this._Access = Access;
		this._Refresh = Refresh;
		this._AccessExpires = AccessExpires;
		this._RefreshExpires = RefreshExpires;
	}

	private static _access?: IDbUserPublicInfo;
	private static _refresh?: IJwtInfo;
	private static _accessExpires?: Date;
	private static _refreshExpires?: Date;
	private static _isCreating = false;
	private static _id = 0;

	static Create = async (): Promise<AuthorizedApiInteractionBase> =>
	{
		this._id++;
		let count = 0;
		while (this._isCreating && count++ < 100)
		{
			console.info(`AApiInteractor[${this._id}] creating is delayed during another call.`);
			await new Promise(resolve => setTimeout(resolve, 100));
		}

		this._isCreating = true;
		var timeout = setTimeout(() => { this._isCreating = false; }, 10000);
		try
		{
			let isAccessValid = false;
			try { isAccessValid = this.CheckAccessValidity(); }
			catch { }

			let nowUnix = Date.now();
			let minAccessExp = new Date(nowUnix + 5 * 1000); // +5 sec
			if (isAccessValid && this._accessExpires! > minAccessExp)
			{
				console.info("AApiInteractor created: access token is still valid.");
				return new AuthorizedApiInteractionBase(
					this._access!, this._refresh!,
					this._accessExpires!, this._refreshExpires!);
			}

			let isRefreshValid = false;
			try { isRefreshValid = this.CheckRefreshValidity(); }
			catch { }
			if (isRefreshValid)
			{
				await this.PerformRefresh();

				if (!this.CheckAccessValidity())
				{
					// Refresh failed
					throw new Error("Error creating AApiInteractor: access token is invalid after refresh.");
				}
				else
				{
					// Refresh successfully
					console.info("AApiInteractor created: refresh successfully.");
					return new AuthorizedApiInteractionBase(
						this._access!, this._refresh!,
						this._accessExpires!, this._refreshExpires!)
				}
			} else
			{
				// Refresh is invalid
				throw new Error("Error while creating AApiInteractor: both access and refresh token are invalid.");
			}
		} finally
		{
			this._isCreating = false;
			clearTimeout(timeout);
		}
	}

	private static CheckAccessValidity = () =>
	{
		let readed = localStorage.getItem(Constants.AccessTokenName);
		let parsed = jwtDecode<IDbUserPublicInfo>(readed!);
		let dates = JwtHelper.GetLifetime(parsed);

		this._accessExpires = dates.exp;
		this._access = parsed;

		return JwtHelper.ValidateParsedLifetime(dates.nbf, dates.exp);
	}
	private static CheckRefreshValidity = () =>
	{
		let readed = localStorage.getItem(Constants.RefreshTokenName);
		let parsed = jwtDecode<IJwtInfo>(readed!);
		let dates = JwtHelper.GetLifetime(parsed);

		this._refreshExpires = dates.exp;
		this._refresh = parsed;

		return JwtHelper.ValidateParsedLifetime(dates.nbf, dates.exp);
	}
	private static PerformRefresh = async () =>
	{
		let response = await axios.patch<JwtResponse>(UrlHelper.Backend.V1.Auth.Patch.Refresh);

		if (!Common.Between(200, response.status, 299)) return false;

		localStorage.setItem(Constants.AccessTokenName, response.data.access);
		localStorage.setItem(Constants.RefreshTokenName, response.data.refresh);
		return true;
	}
}