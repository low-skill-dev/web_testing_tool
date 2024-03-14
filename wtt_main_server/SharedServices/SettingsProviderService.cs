using System.Runtime.InteropServices;
using System.Security.Cryptography;
using Microsoft.Extensions.Configuration;
using Models.ServicesSettings;
using static System.Runtime.InteropServices.RuntimeInformation;

namespace wtt_main_server_services;


/* Sigleton-сервис, служит для повышения уровня абстракции в других сервисах.
 * Обеспечивает получение настроек из appsettings и прочих файлов
 * с последующей их записью в соответствующие модели.
 */
public class SettingsProviderService
{
	protected readonly IConfiguration _configuration;

	public virtual JwtServiceSettings WttJwtServiceSettings =>
		this._configuration.GetSection(nameof(this.WttJwtServiceSettings))
		.Get<JwtServiceSettings>() ?? new();

	public virtual ECDsaFilesLocations ECDsaFilesLocations =>
		this._configuration.GetSection(nameof(this.ECDsaFilesLocations))
		.Get<ECDsaFilesLocations>() ?? new();

	public virtual AuthControllerSettings AuthControllerSettings =>
		this._configuration.GetSection(nameof(this.AuthControllerSettings))
		.Get<AuthControllerSettings>() ?? new();

	private static readonly string JwtSigningECDsaLock = DateTime.UtcNow.ToString();
	public virtual ECDsa JwtSigningECDsa
	{
		get
		{
			var ret = ECDsa.Create();
			var path = this.ECDsaFilesLocations.Jwt;
			if(IsOSPlatform(OSPlatform.Windows)) path = "C:" + path;
			var file = new FileInfo(path);

			if(!file.Exists || file.Length < 256)
			{
				Monitor.TryEnter(JwtSigningECDsaLock, TimeSpan.FromSeconds(10));
				try
				{
					if(!file.Exists || file.Length < 256)
					{
						ret.GenerateKey(ECCurve.NamedCurves.nistP521);
						File.WriteAllText(path, ret.ExportPkcs8PrivateKeyPem());
						// do not return here, let it raise an exception if the file can't be read
					}
				}
				finally
				{
					Monitor.Exit(JwtSigningECDsaLock);
				}
			}

			ret.ImportFromPem(File.ReadAllText(path));
			return ret;
		}
	}

	public SettingsProviderService(IConfiguration configuration)
	{
		this._configuration = configuration;
	}
}
