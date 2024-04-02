import DbConditionalAction from "../Actions/DbConditionalAction";
import DbEchoAction from "../Actions/DbEchoAction";
import DbHttpAction from "../Actions/DbHttpAction";

export interface ActionsCollection
{
	//dbGetParametersActions: DbGetParametersAction[];
	//dbCertificateActions: DbCertificateAction[];
	dbConditionalActions: DbConditionalAction[];
	//dbScenarioActions: DbScenarioAction[];
	//dbDelayActions: DbDelayAction[];
	//dbErrorActions: DbErrorAction[];
	dbEchoActions: DbEchoAction[];
	dbHttpActions: DbHttpAction[];
	//dbImapActions: DbImapAction[];
}