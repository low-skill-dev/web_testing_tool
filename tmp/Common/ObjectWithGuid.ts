import ObjectWithDates from "./ObjectWithDates";

export default abstract class ADbObjectWithGuid extends ObjectWithDates
{
	Guid: string = new Date().toISOString();
}