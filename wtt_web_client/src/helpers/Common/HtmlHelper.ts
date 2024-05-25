export default class HtmlHelper
{
	static FixTextAreaHeight = (e: any) =>
	{
		e.currentTarget.style.height = ""; 
		e.currentTarget.style.height = `calc(${e.currentTarget.scrollHeight}px + 5px)`;
	}
}