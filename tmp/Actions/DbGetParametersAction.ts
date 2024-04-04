import ADbWebRequest from "./Abstract/ADbWebRequest";

export default interface DbGetParametersAction extends ADbWebRequest
{
	type: string;
}