using cloudscribe.Web.Localization;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using System.Globalization;
using System.Linq;

//https://github.com/aspnet/Localization/issues/157

namespace localization.WebApp
{
    public class Startup
    {

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;

        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<GlobalResourceOptions>(Configuration.GetSection("GlobalResourceOptions"));
            services.AddSingleton<IStringLocalizerFactory, GlobalResourceManagerStringLocalizerFactory>();
            //services.AddSingleton<IStringLocalizerFactory, PatchedResourceManagerStringLocalizerFactory>();

            //services.AddLocalization();
            services.AddLocalization(options => options.ResourcesPath = "GlobalResources");

            services.AddMvc()
                .AddRazorOptions(options =>
                {
                    options.AddEmbeddedViewsForWebLib();
                })
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization()
                ;

            var supportedCultures = new[]
                {
                    new CultureInfo("en-US"),
                    //new CultureInfo("en"),
                    new CultureInfo("fr-FR"),
                   // new CultureInfo("fr"),
                };

            var routeSegmentLocalizationProvider = new FirstUrlSegmentRequestCultureProvider(supportedCultures.ToList());

            // this seems in conflict with middleware config below where we new up the options
            // but without this the language dropdown in layout only shows english
            services.Configure<RequestLocalizationOptions>(options =>
            {


                // State what the default culture for your application is. This will be used if no specific culture
                // can be determined for a given request.
                options.DefaultRequestCulture = new RequestCulture(culture: "en-US", uiCulture: "en-US");

                // You must explicitly state which cultures your application supports.
                // These are the cultures the app supports for formatting numbers, dates, etc.
                options.SupportedCultures = supportedCultures;

                // These are the cultures the app supports for UI strings, i.e. we have localized resources for.
                options.SupportedUICultures = supportedCultures;

                // You can change which providers are configured to determine the culture for requests, or even add a custom
                // provider with your own logic. The providers will be asked in order to provide a culture for each request,
                // and the first to provide a non-null result that is in the configured supported cultures list will be used.
                // By default, the following built-in providers are configured:
                // - QueryStringRequestCultureProvider, sets culture via "culture" and "ui-culture" query string values, useful for testing
                // - CookieRequestCultureProvider, sets culture via "ASPNET_CULTURE" cookie
                // - AcceptLanguageHeaderRequestCultureProvider, sets culture via the "Accept-Language" request header
                //options.RequestCultureProviders.Insert(0, new CustomRequestCultureProvider(async context =>
                //{
                //  // My custom request culture logic
                //  return new ProviderCultureResult("en");
                //}));
                options.RequestCultureProviders.Insert(0, routeSegmentLocalizationProvider);

            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app,
            IHostingEnvironment env,
            ILoggerFactory loggerFactory,
            IOptions<RequestLocalizationOptions> locOptions
            )
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();

            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();


            app.UseRequestLocalization(locOptions.Value);

            // Add external authentication middleware below. To configure them please see http://go.microsoft.com/fwlink/?LinkID=532715


            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default-localized",
                    template: "{culture}/{controller}/{action}/{id?}",
                    defaults: new { controller = "Home", action = "Index" },
                    constraints: new { culture = new CultureSegmentRouteConstraint() }
                    );


                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
