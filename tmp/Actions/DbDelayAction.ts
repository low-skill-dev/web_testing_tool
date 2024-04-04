import ADbAction from "./Abstract/ADbAction";

export default interface DbDelayAction extends ADbAction
{
	type: string;
	delayMs: number;
}