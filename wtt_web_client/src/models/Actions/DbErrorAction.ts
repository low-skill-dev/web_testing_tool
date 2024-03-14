import ADbAction from "./Abstract/ADbAction";

export default interface DbErrorAction extends ADbAction
{
	type: string;
	message: string;
	isCritical: boolean;
}