import DbCertificateAction from './DbCertificateAction.js';
import DbConditionalAction from './DbConditionalAction.js';
import DbDelayAction from './DbDelayAction.js';
import DbEchoAction from './DbEchoAction.js';
import DbErrorAction from './DbErrorAction.js';
import DbGetParametersAction from './DbGetParametersAction.js';
import DbHttpAction from './DbHttpAction.js';
import DbImapAction from './DbImapAction.js';
import DbScenarioAction from './DbScenarioAction.js';

export default interface ActionsCollection
{
	dbGetParametersActions: DbGetParametersAction[];
	dbCertificateActions: DbCertificateAction[];
	dbConditionalActions: DbConditionalAction[];
	dbScenarioActions: DbScenarioAction[];
	dbDelayActions: DbDelayAction[];
	dbErrorActions: DbErrorAction[];
	dbEchoActions: DbEchoAction[];
	dbHttpActions: DbHttpAction[];
	dbImapActions: DbImapAction[];
}