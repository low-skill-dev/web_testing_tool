import ADbAction from "./Abstract/ADbAction";

export default interface DbConditionalAction extends ADbAction
{
	type: string;
	jsBoolExpression: string;
	actionOnTrue: string | null;
	actionOnFalse: string | null;
}