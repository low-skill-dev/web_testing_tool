//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Builder;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.AspNetCore.HttpsPolicy;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Hosting;
//using Microsoft.Extensions.Logging;
//using wtt_main_server_api.Database;
//using Models.ServicesSettings;
//using wtt_main_server_services;

//namespace wtt_main_server_api;

//public class Startup
//{
//	public Startup(IConfiguration configuration)
//	{
//		Configuration = configuration;
//	}

//	public IConfiguration Configuration { get; }

//	// This method gets called by the runtime. Use this method to add services to the container.
//	public void ConfigureServices(IServiceCollection services)
//	{
//		services.AddControllers();
//		services.AddDbContext<WttContext>(opts =>
//		{
//			opts.UseNpgsql(builder.Environment.IsDevelopment()
//				? services.Configuration["ConnectionStrings:LocalhostConnection"]
//				: builder.Configuration["ConnectionStrings:DatabaseConnection"]
//				, opts => { opts.MigrationsAssembly(nameof(main_server_api)); });
//		});
//		services.AddSingleton<SettingsProviderService>();
//		services.AddSingleton<WttJwtService>(sp =>
//		{
//			var cert = sp.GetRequiredService<SettingsProviderService>().JwtSigningECDsa;
//			var logger = sp.GetRequiredService<ILogger<WttJwtService>>();
//			return new WttJwtService(cert, logger);
//		});

//		services.AddScoped<AuthControllerSettings>(sp =>
//			sp.GetRequiredService<SettingsProviderService>().AuthControllerSettings);
//	}

//	// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
//	public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
//	{
//		if(env.IsDevelopment())
//		{
//			app.UseDeveloperExceptionPage();
//		}


//		app.UseRouting();
//		app.UseEndpoints(e => e.MapControllers());
//	}
//}
