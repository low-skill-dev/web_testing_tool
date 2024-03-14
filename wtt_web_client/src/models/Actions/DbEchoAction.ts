import ADbWebRequest from "./Abstract/ADbWebRequest";

export default interface DbEchoAction extends ADbWebRequest
{
	type: string;
}