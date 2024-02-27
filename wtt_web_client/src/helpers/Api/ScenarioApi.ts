import AuthorizedApiInteractionBase from './AuthorizedApiInteractionBase';
import DbTestScenario from '../../models/Scenario/DbTestScenario';
import UrlHelper from "./UrlHelper";
import axios from 'axios';
import Common from '../Common/Common';

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

	public GetMyScenarios = async () =>
	{
		const res = await axios.post<DbTestScenario[]>(UrlHelper.Backend.V1.Scenario.Get.GetMyScenarios);
		return Common.Between(200, res.status, 299) ? res.data : null;
	}
}