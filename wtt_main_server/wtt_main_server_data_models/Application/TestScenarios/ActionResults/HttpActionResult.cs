using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wtt_main_server_data.Application.Abstract;

namespace wtt_main_server_data.Application.TestScenarios.ActionResults;

public class HttpActionResult : AWebActionResult
{
	public string? RequestBody { get; set; }
	public Dictionary<string, string>? RequestCookies { get; set; }
	public Dictionary<string, string>? RequestHeaders { get; set; }

	public string? ResponseBody { get; set; }
	public Dictionary<string, string>? ResponseCookies { get; set; }
	public Dictionary<string, string>? ResponseHeaders { get; set; }

	// TODO: public List<byte[]> ResponseFiles { get; set; }
}
