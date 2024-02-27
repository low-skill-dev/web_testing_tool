export default class Common
{
	public static Between = (left: number, value: number, right: number) =>
	{
		return left <= value && value <= right;
	}
}