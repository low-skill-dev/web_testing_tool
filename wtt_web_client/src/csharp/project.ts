//     This code was generated by a Reinforced.Typings tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.

export enum ActionTypes {
	DbCertificateActionType = 0,
	DbConditionalActionType = 1,
	DbDelayActionType = 2,
	DbEchoActionType = 3,
	DbErrorActionType = 4,
	DbGetParametersActionType = 5,
	DbHttpActionType = 6,
	DbImapActionType = 7,
	DbScenarioActionType = 8,
	DbTestScenarioType = 9
}
export enum UserRoles {
	Regular = 128,
	Moderator = 32768,
	Administrator = 4194304,
	CEO = 536870912
}
export enum HttpTlsValidationMode {
	Enabled = 0,
	AllowSelfSigned = 1,
	Disabled = 2
}
export enum HttpRequestMethod {
	Get = 1,
	Post = 2,
	Put = 4,
	Patch = 8,
	Delete = 16
}
export class ObjectWithDates
{
	public Created: any;
	public Changed?: any;
}
export class ObjectWithGuid extends ObjectWithDates
{
	public Guid?: string;
}
export class ObjectWithUser extends ObjectWithGuid
{
	public UserGuid?: any;
}
export abstract class ADbAction extends ObjectWithGuid
{
	public Type?: ActionTypes;
	public Name: string = 'Action';
	public Next?: any;
	public ColumnId: number = 0;
	public RowId: number = 0;
	public Bypass: boolean = false;
	public ContinueExecutionInCaseOfCriticalError: boolean = false;
	public AfterRunScript?: string;
}
export abstract class ADbProxiedAction extends ADbAction
{
	public ProxyUrl?: string;
}
export abstract class ADbWebRequest extends ADbProxiedAction
{
	public RequestUrl: string = '';
}
export abstract class ADbHttpAction extends ADbWebRequest
{
	constructor ()
	{
		super();
		this.Method = HttpRequestMethod.Get;
		this.TlsValidationMode = HttpTlsValidationMode.Enabled
	}
	public Method: HttpRequestMethod;
	public TlsValidationMode: number;
}
export class DbCertificateAction extends ADbHttpAction
{
	constructor ()
	{
		super();
		this.Type = ActionTypes.DbCertificateActionType;
	}
	public Type: ActionTypes;
}
export class DbConditionalAction extends ADbAction
{
	constructor ()
	{
		super();
		this.Type = ActionTypes.DbConditionalActionType;
	}
	public Type: ActionTypes;
	public JsBoolExpression: string = '';
	public ActionOnTrue?: any;
	public ActionOnFalse?: any;
}
export class DbDelayAction extends ADbAction
{
	constructor ()
	{
		super();
		this.Type = ActionTypes.DbDelayActionType;
	}
	public Type: ActionTypes;
	public DelayMs: number = 250;
}
export class DbEchoAction extends ADbWebRequest
{
	constructor ()
	{
		super();
		this.Type = ActionTypes.DbEchoActionType;
	}
	public Type: ActionTypes;
}
export class DbGetParametersAction extends ADbWebRequest
{
	constructor ()
	{
		super();
		this.Type = ActionTypes.DbGetParametersActionType;
	}
	public Type: ActionTypes;
}
export class DbHttpAction extends ADbHttpAction
{
	constructor ()
	{
		super();
		this.Type = ActionTypes.DbHttpActionType;
	}
	public Type: ActionTypes;
	public RequestBody?: string;
	public RequestHeaders?: { [key:string]: string };
	public RequestCookies?: { [key:string]: string };
	public VariableToPath?: { [key:string]: string };
	public VariablesUpdatedInTryBlock?: string[];
}
export class DbImapAction extends ADbAction
{
	constructor ()
	{
		super();
		this.Type = ActionTypes.DbImapActionType;
	}
	public Type: ActionTypes;
	public UserImapAccountGuid: any;
	public SubjectRegex?: string;
	public SenderRegex?: string;
	public BodyRegex?: string;
	public BodySearchRegex?: string;
	public BodyProcessingScript?: string;
	public MinSearchLength: number = 4;
	public MaxSearchLength: number = 8;
	public SearchMustContain: string[] = [];
}
export class DbLogAction extends ADbAction
{
	constructor ()
	{
		super();
		this.Type = ActionTypes.DbErrorActionType;
	}
	public Type: ActionTypes;
	public Message: string = '';
	public StopExecution: boolean = false;
}
export class ChangePasswordRequest
{
	public Password: string = '';
}
export class RegistrationRequest extends ChangePasswordRequest
{
	public Email: string = '';
}
export class LoginRequest extends RegistrationRequest
{
	public TotpCode?: string;
}
export interface IJwtResponse
{
	Access: string;
	Refresh: string;
}
export class DbScenarioAction extends ADbAction
{
	constructor ()
	{
		super();
		this.Type = ActionTypes.DbTestScenarioType;
	}
	public Type: ActionTypes;
	public CalledScenarioGuid: any;
	public Arguments: Map<string,string> = new Map<string, string>();
	public WriteAllResultToVariable?: string;
}
export class DbTestScenario extends ObjectWithUser
{
	public Name?: string;
	public EnableEmailNotifications?: boolean;
	public ActionsJson?: ActionsCollection;
	public EntryPoint?: any;
	public ArgTypes?: number[];
	public ArgNames?: string[];
	public RunIntervalMinutes?:number;
}
export class ActionsCollection
{
	public DbGetParametersActions?: DbGetParametersAction[];
	public DbCertificateActions?: DbCertificateAction[];
	public DbConditionalActions?: DbConditionalAction[];
	public DbScenarioActions?: DbScenarioAction[];
	public DbDelayActions?: DbDelayAction[];
	public DbErrorActions?: DbLogAction[];
	public DbEchoActions?: DbEchoAction[];
	public DbHttpActions?: DbHttpAction[];
	public DbImapActions?: DbImapAction[];
}
export interface IDbUserPublicInfo extends ObjectWithGuid
{
	Role: UserRoles;
	IsDisabled: boolean;
	RegistrationDate: any;
	Email: string;
	EmailConfirmedAtUtc?: any;
	IsEmailConfirmed: boolean;
	PasswordLastChanged: any;
}
export interface IDbScenarioRun extends ObjectWithGuid
{
	ScenarioGuid: any;
	Started?: any;
	Completed?: any;
	IsSucceeded?: boolean;
	ErrorMessage?: string;
	ProcessorTime?: any;
	RunReason?: number;
	ScenarioJsonSnapshot?: string;
	ScenarioJsonResult?: string;
	InputValues?: Map<string,string>;
	OutputValues?: Map<string,string>;
}
