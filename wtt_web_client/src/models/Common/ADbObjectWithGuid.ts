import ADbObjectWithId from "./ADbObjectWithId";

export default interface ADbObjectWithGuid extends ADbObjectWithId
{
	guid: string;
}