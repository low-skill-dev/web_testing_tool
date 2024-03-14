import ObjectWithDates from "./ADbObjectWithDates";

export default interface ADbObjectWithGuid extends ObjectWithDates
{
	guid: string;
}