import ADbAction from "./Abstract/ADbAction";

export default interface DbScenarioAction extends ADbAction
{
	type: string;
	calledScenarioGuid: string;
	stopExecutionOnInternalError: boolean;
	arguments: { [key: string]: string; };
	writeAllResultToVariable: string | null;
	variablesExtractedFromResult: { [key: string]: string; };
}