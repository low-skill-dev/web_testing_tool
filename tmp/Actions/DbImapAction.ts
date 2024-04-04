import ADbAction from "./Abstract/ADbAction";

export default interface DbImapAction extends ADbAction
{
	type: string;
	userImapAccountGuid: string;
	subjectRegex: string | null;
	senderRegex: string | null;
	bodyRegex: string | null;
	bodySearchRegex: string | null;
	bodyProcessingScript: string | null;
	minSearchLength: number;
	maxSearchLength: number;
	searchMustContain: string[];
}