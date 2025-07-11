using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using System.Collections.Generic;
using System.Globalization;

namespace LoggingCachingLocalization
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.Configure<CookiePolicyOptions>(options =>
			{
				// This lambda determines whether user consent for non-essential cookies is needed for a given request.
				options.CheckConsentNeeded = context => true;
				options.MinimumSameSitePolicy = SameSiteMode.None;
			});

			services.AddMemoryCache();

			services.AddDistributedRedisCache(options =>
			{
				options.Configuration = "localhost:6379";
				options.InstanceName = "master";
			});

			services.Configure<RequestLocalizationOptions>(options =>
			{
				var supportedCultures = new List<CultureInfo>
				{
					new CultureInfo("tr-TR"),
					new CultureInfo("en-US")
				};

				options.DefaultRequestCulture = new RequestCulture("tr-TR");
				options.SupportedCultures = supportedCultures;
				options.SupportedUICultures = supportedCultures;
			});

			services.AddLocalization(options =>
			{
				options.ResourcesPath = "Resources";
			});

			services.AddMvc()
				.SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
			.AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
			.AddDataAnnotationsLocalization(options =>
			{
				options.DataAnnotationLocalizerProvider = (type, factory) =>
				{
					return factory.Create(typeof(SharedResource));
				};
			});

		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();
			app.UseCookiePolicy();

			app.UseMvc(routes =>
			{
				routes.MapRoute(
					name: "default",
					template: "{controller=Home}/{action=Index}/{id?}");
			});
		}
	}
}
