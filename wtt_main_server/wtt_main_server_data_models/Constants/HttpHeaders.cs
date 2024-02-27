using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wtt_main_server_data.Constants;
public class HttpHeaders
{
	public const string
		GeoIpCountryHeaderName = @"X-GeoIP-Country", // $geoip_city_country_name
		GeoIpCityHeaderName = @"X-GeoIP-City", // $geoip_city
		GeoIpLatitudeHeaderName = @"X-GeoIP-Latitude", // $geoip_latitude
		GeoIpLongitudeHeaderName = @"X-GeoIP-Longitude", // $geoip_longitude
		RealIpHeaderName = @"X-Real-IP";
}
