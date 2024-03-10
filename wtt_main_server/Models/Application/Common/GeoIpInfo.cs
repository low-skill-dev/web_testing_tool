using System.Net;

namespace Models.Application.Common;

public class GeoIpInfo
{
	public IPAddress IpAddress { get; set; } = null!;
	public double? Longitude { get; set; }
	public double? Latitude { get; set; }
	public string? Country { get; set; }
	public string? City { get; set; }
}
