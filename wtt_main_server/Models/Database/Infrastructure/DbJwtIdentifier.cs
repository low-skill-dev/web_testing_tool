using CommonLibrary.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Models.Database.Infrastructure;

#pragma warning disable CS8618

public class DbJwtIdentifier : ObjectWithUser
{
	[MinLength(512 / 8)]
	[MaxLength(512 / 8)]
	public required byte[] JtiSha512 { get; set; }
	public required DateTime IssuedAt { get; set; }
	public required DateTime OriginIssuedAt { get; set; } // when the very first token was issued
	public required IPAddress? IPAddress { get; set; } // provided by nginx :: proxy_set_header X-Real-IP $remote_addr
	public required string? Country { get; set; } // provided by nginx :: proxy_set_header X-GeoIP-Country $geoip_country_name;
	public required string? City { get; set; } // provided by nginx :: proxy_set_header X-GeoIP-Country $geoip_city;
}