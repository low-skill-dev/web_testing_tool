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
using WebApi.Database;
using Models.ServicesSettings;
using wtt_main_server_services;
using Microsoft.EntityFrameworkCore;
using CommonLibrary.Services.Jwt;
using Microsoft.Extensions.DependencyInjection;
using WebApi.Infrastructure.Authorization;
using SharedServices;
using Newtonsoft.Json.Serialization;
using WebApi.Services;

namespace WebApi;

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
			.AddJsonFile("/etc/wtt/secrets.json", Environment.OSVersion.Platform == PlatformID.Win32NT)
			.AddEnvironmentVariables()
			.Build();


		builder.Services.AddControllers()
			//	.AddNewtonsoftJson(opts =>
			//{
			//	opts.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
			//	opts.SerializerSettings.ContractResolver = new DefaultContractResolver();
			//})
			.AddJsonOptions(opts =>
		{
			opts.JsonSerializerOptions.WriteIndented = true;
			opts.JsonSerializerOptions.AllowTrailingCommas = true;
			opts.JsonSerializerOptions.PropertyNameCaseInsensitive = false;
			opts.JsonSerializerOptions.PropertyNamingPolicy = null;
			opts.JsonSerializerOptions.PreferredObjectCreationHandling = System.Text.Json.Serialization.JsonObjectCreationHandling.Populate;
			opts.JsonSerializerOptions.ReadCommentHandling = System.Text.Json.JsonCommentHandling.Skip;
			opts.JsonSerializerOptions.UnmappedMemberHandling = System.Text.Json.Serialization.JsonUnmappedMemberHandling.Skip;
		});

		builder.Services.AddDbContext<WttContext>(opts =>
		{
			opts.UseNpgsql(builder.Environment.IsDevelopment()
				? builder.Configuration["ConnectionStrings:LocalhostConnection"]
				: builder.Configuration["ConnectionStrings:DatabaseConnection"]);
		});
		builder.Services.AddSingleton<SettingsProviderService>();
		builder.Services.AddSingleton<IJwtService, EcdsaJwtService>(sp =>
		{
			var pr = sp.GetRequiredService<SettingsProviderService>();
			var logger = sp.GetRequiredService<ILogger<EcdsaJwtService>>();
			return new(pr.JwtSigningECDsa, logger);
		});
		builder.Services.AddSingleton<WttJwtService>(sp =>
		{
			var pr = sp.GetRequiredService<SettingsProviderService>();
			var logger = sp.GetRequiredService<ILogger<WttJwtService>>();
			var baseJwt = sp.GetRequiredService<IJwtService>();
			return new WttJwtService(baseJwt, pr.WttJwtServiceSettings, logger);
		});

		builder.Services.AddScoped<JwtServiceSettings>(sp =>
			sp.GetRequiredService<SettingsProviderService>().WttJwtServiceSettings);

		builder.Services.AddScoped<JwtMinIatStorage>();
		builder.Services.AddScoped<JwtParseMiddleware>();

		builder.Services
			.AddSingleton<ScenarioSchedulerBackgroundService>(sp =>
				new ScenarioSchedulerBackgroundService(
					sp.CreateScope().ServiceProvider.GetRequiredService<WttContext>()))
			.AddHostedService(sp => sp.GetRequiredService<ScenarioSchedulerBackgroundService>());

		builder.Services
			.AddSingleton<ScenarioExecutorBackgroundService>(sp =>
				new ScenarioExecutorBackgroundService(
					sp.CreateScope().ServiceProvider.GetRequiredService<WttContext>(), sp,
					sp.CreateScope().ServiceProvider.GetRequiredService<ScenarioSchedulerBackgroundService>(),
					sp.CreateScope().ServiceProvider.GetRequiredService<ILogger<ScenarioExecutorBackgroundService>>()))
			.AddHostedService(sp => sp.GetRequiredService<ScenarioExecutorBackgroundService>());


		var app = builder.Build();

		if(builder.Environment.IsDevelopment())
		{
			app.UseDeveloperExceptionPage();
		}


		app.UseRouting();
		app.UseMiddleware<JwtParseMiddleware>();
		app.MapControllers();

		//app.Services.CreateScope().ServiceProvider.GetRequiredService<WttContext>().Database.EnsureDeleted();
		app.Services.CreateScope().ServiceProvider.GetRequiredService<WttContext>().Database.Migrate();
		//app.Services.CreateScope().ServiceProvider.GetRequiredService<WttContext>().CreateTriggers();
		app.Run();
	}
}
