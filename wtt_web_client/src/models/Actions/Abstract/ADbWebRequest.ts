import ADbProxiedAction from "./ADbProxiedAction";

export default interface ADbWebRequest extends ADbProxiedAction
{
	requestUrl: string;
}