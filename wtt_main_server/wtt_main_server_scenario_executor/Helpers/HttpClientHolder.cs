using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using static System.Security.Authentication.SslProtocols;

namespace wtt_main_server_scenario_executor.Helpers;
public class HttpClientHolder
{
	public static HttpClient WithAllValidations { get; }
	public static HttpClient WithAllowedSelfSigned { get; }
	public static HttpClient WithAllowedNoTls { get; }

	static HttpClientHolder()
	{
		WithAllValidations = CreateClientWithAllValidations();
		WithAllowedSelfSigned = CreateClientWithAllowedSelfSigned();
		WithAllowedNoTls = CreateClientWithAllowedNoTls();
	}

	private static HttpClientHandler CreateHandlerWithAllValidations()
	{
		return new HttpClientHandler
		{
			SslProtocols = Tls12 | Tls13,
			CheckCertificateRevocationList = true,
			MaxRequestContentBufferSize = 32 * 1024 * 1024, // 32 MiB
		};
	}
	private static HttpClientHandler CreateHandlerWithAllowedSelfSigned()
	{
		return new HttpClientHandler
		{
			SslProtocols = Tls12 | Tls13,
			CheckCertificateRevocationList = false,
			ServerCertificateCustomValidationCallback = VerifySelfSigned,
		};
	}
	private static HttpClientHandler CreateHandlerWithAllowedNoTls()
	{
		return new HttpClientHandler
		{
			SslProtocols = MergeAllProtocols(),
			CheckCertificateRevocationList = false,
			ServerCertificateCustomValidationCallback = (_, _, _, _) => true,
		};
	}
	private static SslProtocols MergeAllProtocols()
	{
#pragma warning disable CS0618, SYSLIB0039
		return Ssl2 | Ssl3 | Tls | Tls11 | Tls12 | Tls13;
#pragma warning restore CS0618, SYSLIB0039
	}
	private static HttpClient CreateClientWithAllValidations()
	{
		return new(CreateHandlerWithAllValidations(), true);
	}
	private static HttpClient CreateClientWithAllowedSelfSigned()
	{
		return new(CreateHandlerWithAllowedSelfSigned(), true);
	}
	private static HttpClient CreateClientWithAllowedNoTls()
	{
		return new(CreateHandlerWithAllowedNoTls(), true);
	}
	private static bool VerifySelfSigned(object sender, X509Certificate? cert, X509Chain? chain, SslPolicyErrors errors)
	{
		// https://stackoverflow.com/a/56057732
		if(errors == SslPolicyErrors.None) return true;
		if(cert is null || chain is null) return false;

		chain.ChainPolicy.DisableCertificateDownloads = true;
		chain.ChainPolicy.RevocationMode = X509RevocationMode.NoCheck;
		chain.ChainPolicy.VerificationFlags = X509VerificationFlags.AllowUnknownCertificateAuthority;
		return chain.Build((X509Certificate2)cert);
	}
}
