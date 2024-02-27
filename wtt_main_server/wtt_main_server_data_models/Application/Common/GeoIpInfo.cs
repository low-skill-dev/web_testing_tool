using System.Net;

namespace wtt_main_server_data.Application.Common;

public class GeoIpInfo
{
	public double? Latitude { get; set; }
	public double? Longitude { get; set; }
	public string? Country { get; set; }
	public string? City { get; set; }
	public IPAddress? IpAddress { get; set; }
}
