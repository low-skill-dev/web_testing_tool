import AuthorizedApiInteractionBase from './AuthorizedApiInteractionBase';
import UrlHelper from "./UrlHelper";
import axios from 'axios';
import Common from '../Common/Common';
import { DbTestScenario, IDbScenarioRun } from "src/csharp/project";

export default class ScenarioApi
{
	private _authedApi: AuthorizedApiInteractionBase;

	private constructor(authedApi: AuthorizedApiInteractionBase)
	{
		this._authedApi = authedApi;
	}

	static Create = async () =>
	{
		return new ScenarioApi(await AuthorizedApiInteractionBase.Create());
	}

	public GetMyScenarios = async (byScenarioGuid?: string, byUserGuid?: string) =>
	{
		let queryArr = new Array<string>();
		if (byScenarioGuid) queryArr.push(`guid=${byUserGuid}`);
		if (byUserGuid) queryArr.push(`owner=${byUserGuid}`);

		let query = queryArr.length > 0 ? "?" + queryArr.join("&") : "";

		const res = await axios.get<DbTestScenario[]>(UrlHelper.Backend.V1.Scenario.Get.GetScenarios + query);
		return Common.Between(200, res.status, 299) ? res.data : null;
	}

	public GetMyLogs = async () =>
	{
		const res = await axios.get<ScenarioGuidToRuns[]>(UrlHelper.Backend.V1.Scenario.Get.GetLogs);
		return Common.Between(200, res.status, 299) ? res.data : null;
	}

	public SaveScenarios = async (data: DbTestScenario[]) => 
	{
		const res = await axios.put(UrlHelper.Backend.V1.Scenario.Put.SaveScenarios, data);
		return res.status;
	}
}

export type ScenarioGuidToRuns = {
	g?: string;
	r?: IDbScenarioRun[];
};