export default interface DbHttpAction extends ADbHttpAction
{
	type: string;
	requestBody: string | null;
	requestHeaders: { [key: string]: string; } | null;
	requestCookies: { [key: string]: string; } | null;
	userScript: string | null;
	useTryBlockForUserScript: boolean;
	variableToPath: { [key: string]: string; } | null;
	variablesUpdatedInTryBlock: string[] | null;
}