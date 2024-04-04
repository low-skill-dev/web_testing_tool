import ADbAction from './Abstract/ADbAction.js';
import DbCertificateAction from './DbCertificateAction.js';
import DbConditionalAction from './DbConditionalAction.js';
import DbDelayAction from './DbDelayAction.js';
import DbEchoAction from './DbEchoAction.js';
import DbErrorAction from './DbErrorAction.js';
import DbGetParametersAction from './DbGetParametersAction.js';
import DbHttpAction from './DbHttpAction.js';
import DbImapAction from './DbImapAction.js';
import DbScenarioAction from './DbScenarioAction.js';

export default class ActionsCollection
{
	dbGetParametersActions!: DbGetParametersAction[];
	dbCertificateActions!: DbCertificateAction[];
	dbConditionalActions!: DbConditionalAction[];
	dbScenarioActions!: DbScenarioAction[];
	dbDelayActions!: DbDelayAction[];
	dbErrorActions!: DbErrorAction[];
	dbEchoActions!: DbEchoAction[];
	dbHttpActions!: DbHttpAction[];
	dbImapActions!: DbImapAction[];

	public Enumerate = (): Array<ADbAction> =>
	{
		return (this.dbGetParametersActions as Array<ADbAction>).concat(
			this.dbCertificateActions as Array<ADbAction>,
			this.dbConditionalActions as Array<ADbAction>,
			this.dbScenarioActions as Array<ADbAction>,
			this.dbDelayActions as Array<ADbAction>,
			this.dbErrorActions as Array<ADbAction>,
			this.dbEchoActions as Array<ADbAction>,
			this.dbHttpActions as Array<ADbAction>,
			this.dbImapActions as Array<ADbAction>
		);
	};
}