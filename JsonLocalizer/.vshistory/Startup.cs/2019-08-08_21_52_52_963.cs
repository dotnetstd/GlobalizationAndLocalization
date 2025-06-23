using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using System.Globalization;

namespace JsonLocalizer
{
	public class Startup
	{
		public Startup(IConfiguration configuration, IHostingEnvironment environment)
		{
			Configuration = configuration;
			Environment = environment;
		}

		public IConfiguration Configuration { get; }
		public IHostingEnvironment Environment { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.Configure<CookiePolicyOptions>(options =>
			{
				// This lambda determines whether user consent for non-essential cookies is needed for a given request.
				options.CheckConsentNeeded = context => true;
				options.MinimumSameSitePolicy = SameSiteMode.None;
			});

			if (Environment.IsProduction())
			{
				services.AddDistributedRedisCache(opts =>
				{
					opts.Configuration = "localhost";
					opts.InstanceName = "Anvyl.JsonLocalizer";
				});
			}

			if (Environment.IsDevelopment())
				services.AddDistributedMemoryCache();

			services.Configure<JsonLocalizerOptions>(Configuration.GetSection(nameof(JsonLocalizerOptions)));
			services.AddSingleton<IStringLocalizerFactory, JsonStringLocalizerFactory>();

			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
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

			var options = new RequestLocalizationOptions
			{
				DefaultRequestCulture = new RequestCulture(new CultureInfo("en-US")),
				SupportedCultures = new CultureInfo[]{
					new CultureInfo("en-US"),
					new CultureInfo("ro-RO")
				},
				SupportedUICultures = new CultureInfo[]{
					new CultureInfo("en-US"),
					new CultureInfo("ro-RO")
				}
			};
			options.RequestCultureProviders.Insert(0, new CookieRequestCultureProvider());
			app.UseRequestLocalization(options);

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
