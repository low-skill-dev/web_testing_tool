using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Models.Application.Common;
using Models.Constants;

namespace wtt_main_server_helpers;

public class GeoIpHeadersHelper
{
	public static GeoIpInfo GetInfoFromRequest(HttpRequest req)
	{
		var ret = new GeoIpInfo();

		if(req.Headers.TryGetValue(HttpHeaders.GeoIpLatitudeHeaderName, out var t1) 
			&& double.TryParse(t1, out var lat)) ret.Latitude = lat;
		if(req.Headers.TryGetValue(HttpHeaders.GeoIpLongitudeHeaderName, out var t2)
			&& double.TryParse(t2, out var lon)) ret.Longitude = lon;

		if(req.Headers.TryGetValue(HttpHeaders.GeoIpCountryHeaderName, 
			out var t3)) ret.Country = t3;
		if(req.Headers.TryGetValue(HttpHeaders.GeoIpCityHeaderName,
			out var t4)) ret.City = t4;

		if(req.Headers.TryGetValue(HttpHeaders.RealIpHeaderName, out var t5)
			&& IPAddress.TryParse(t5, out var ip)) ret.IpAddress = ip;

		return ret;
	}
}