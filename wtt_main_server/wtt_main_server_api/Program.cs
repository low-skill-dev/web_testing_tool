using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using wtt_main_server_api.Database;
using wtt_main_server_data.ServicesSettings;
using wtt_main_server_services;
using Microsoft.EntityFrameworkCore;

namespace wtt_main_server_api;

public class Program
{
	public static void Main(string[] args)
	{
		var builder = WebApplication.CreateSlimBuilder(args);
		builder.Logging.AddConsole();

		builder.Services.AddDataProtection().UseCryptographicAlgorithms(
			new AuthenticatedEncryptorConfiguration
			{
				EncryptionAlgorithm = EncryptionAlgorithm.AES_256_GCM,
				ValidationAlgorithm = ValidationAlgorithm.HMACSHA512,
			});

		// TODO: took from old project, rewrite for actual files
		builder.Configuration
			.AddJsonFile("./appsettings.json", true)
			.AddJsonFile("/run/secrets/aspsecrets.json", true)
			.AddJsonFile("/run/secrets/nodes.json", true)
			.AddJsonFile("/run/secrets/generated_sig.json", true)
			.AddEnvironmentVariables()
			.Build();

		builder.Services.AddControllers().AddJsonOptions(opts =>
		{
			opts.JsonSerializerOptions.WriteIndented = false;
			opts.JsonSerializerOptions.AllowTrailingCommas = true;
			opts.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
		});

		builder.Services.AddDbContext<WttContext>(opts =>
		{
			opts.UseNpgsql(builder.Environment.IsDevelopment()
				? builder.Configuration["ConnectionStrings:LocalhostConnection"]
				: builder.Configuration["ConnectionStrings:DatabaseConnection"]);
		});
		builder.Services.AddSingleton<SettingsProviderService>();
		builder.Services.AddSingleton<WttJwtService>(sp =>
		{
			var cert = sp.GetRequiredService<SettingsProviderService>().JwtSigningECDsa;
			var logger = sp.GetRequiredService<ILogger<WttJwtService>>();
			return new WttJwtService(cert, logger);
		});

		builder.Services.AddScoped<AuthControllerSettings>(sp =>
			sp.GetRequiredService<SettingsProviderService>().AuthControllerSettings);

		var app = builder.Build();

		if(builder.Environment.IsDevelopment())
		{
			app.UseDeveloperExceptionPage();
		}


		app.UseRouting();
		app.MapControllers();

		app.Services.CreateScope().ServiceProvider.GetRequiredService<WttContext>().Database.EnsureDeleted();
		app.Services.CreateScope().ServiceProvider.GetRequiredService<WttContext>().Database.Migrate();
		app.Services.CreateScope().ServiceProvider.GetRequiredService<WttContext>().CreateTriggers();
		app.Run();
	}
}
