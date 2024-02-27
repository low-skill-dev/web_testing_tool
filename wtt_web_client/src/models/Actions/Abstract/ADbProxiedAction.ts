import ADbAction from "./ADbAction";

export default interface ADbProxiedAction extends ADbAction
{
	proxyGuid: string | null;
}