import { DbTestScenario, ActionsCollection } from "src/csharp/project";

export default class ScenarioHelper
{
	// https://stackoverflow.com/a/43836580/11325184
	public static GetClone = (s: DbTestScenario): DbTestScenario => Object.assign({}, s);
	public static GetClone2 = (s: ActionsCollection): ActionsCollection => Object.assign({}, s);

	public static EnumerateActions(ac: ActionsCollection)
	{
		return [
			...ac.DbGetParametersActions ?? [],
			...ac.DbCertificateActions ?? [],
			...ac.DbConditionalActions ?? [],
			...ac.DbScenarioActions ?? [],
			...ac.DbDelayActions ?? [],
			...ac.DbErrorActions ?? [],
			...ac.DbEchoActions ?? [],
			...ac.DbHttpActions ?? [],
			...ac.DbImapActions ?? []
		];
	}

	public static DeleteAction(ac: ActionsCollection, guid: string)
	{
		ac.DbGetParametersActions = ac.DbGetParametersActions?.filter(x => x.Guid !== guid);
		ac.DbCertificateActions = ac.DbCertificateActions?.filter(x => x.Guid !== guid);
		ac.DbConditionalActions = ac.DbConditionalActions?.filter(x => x.Guid !== guid);
		ac.DbScenarioActions = ac.DbScenarioActions?.filter(x => x.Guid !== guid);
		ac.DbDelayActions = ac.DbDelayActions?.filter(x => x.Guid !== guid);
		ac.DbErrorActions = ac.DbErrorActions?.filter(x => x.Guid !== guid);
		ac.DbEchoActions = ac.DbEchoActions?.filter(x => x.Guid !== guid);
		ac.DbHttpActions = ac.DbHttpActions?.filter(x => x.Guid !== guid);
		ac.DbImapActions = ac.DbImapActions?.filter(x => x.Guid !== guid);
	}

	public static CreateNewActionsCollection = (): ActionsCollection =>
	{
		var ret = new ActionsCollection();
		ret.DbGetParametersActions = [];
		ret.DbCertificateActions = [];
		ret.DbConditionalActions = [];
		ret.DbScenarioActions = [];
		ret.DbDelayActions = [];
		ret.DbErrorActions = [];
		ret.DbEchoActions = [];
		ret.DbHttpActions = [];
		ret.DbImapActions = [];
		return ret;
	}
}