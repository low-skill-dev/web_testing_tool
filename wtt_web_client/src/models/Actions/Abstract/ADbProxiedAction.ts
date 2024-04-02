import ADbAction from "./ADbAction";

export default abstract class ADbProxiedAction extends ADbAction
{
	proxyGuid: string | null = null;
}