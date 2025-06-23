using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;

using System;
using System.Globalization;

using Westwind.AspNetCore.LiveReload;
using Westwind.Globalization;
using Westwind.Globalization.AspnetCore;
using Westwind.Utilities.Properties;

namespace Globalization.Westwind
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


            services.Configure<IdentityOptions>(option =>
            {
                option.Password.RequireDigit = false;
                option.Password.RequiredLength = 6;
                option.Password.RequiredUniqueChars = 0;
                option.Password.RequireLowercase = false;
                option.Password.RequireNonAlphanumeric = false;
                option.Password.RequireUppercase = false;
            });

            services.AddLocalization(options =>
            {
                // I prefer Properties over the default `Resources` folder
                // due to namespace issues if you have a Resources type as
                // most people do for shared resources.
                options.ResourcesPath = "Properties";
            });



            //https://west-wind.com/westwind.globalization/docs/
            //https://github.com/RickStrahl/Westwind.Globalization/blob/master/ChangeLog.md

            //pm > Install - Package Westwind.Globalization.Web
            //pm > Install - Package Westwind.Globalization

            // Optionally enable IStringLocalizer to use DbRes objects instead of default ResourceManager
            // Replace StringLocalizers with Db Resource Implementation
            services.AddSingleton<IStringLocalizerFactory, DbResStringLocalizerFactory>();
            services.AddSingleton<IHtmlLocalizerFactory, DbResHtmlLocalizerFactory>();

            // Required for Westwind.Globalization to work!
            //services.AddWestwindGlobalization();
            // Required: Enable Westwind.Globalization (opt parm is optional)
            // shown here with optional manual configuration code
            services.AddWestwindGlobalization(opt =>
            {
                // the defaults are loaded in this order with later providers overwriting earlier values:
                // 0. Default DbResourceConfiguration values
                // 1. DbResourceConfiguration.json if exists
                // 2. AspNetCore Configuration Manager (IConfiguration/appsettings etc.)
                //    (appsettings.json, environment, user secrets - overrides entire object if set)
                // 3. Settings can be overridden in AddWestwindGlobalization(opt) here

                // Resource Mode - Resx or DbResourceManager
                opt.ResourceAccessMode = ResourceAccessMode.DbResourceManager;  // ResourceAccessMode.Resx

                // *** override provider configuration
                // *** use ConnectionString + DataProvider (or DbResourceManagerType)

                // Sql Server
                // opt.ConnectionString = "server=.;database=localizations;integrated security=true;";
                // opt.ConnectionString = "server=.;database=localizations;uid=localizations;pwd=local;";
                // opt.DataProvider = DbResourceProviderTypes.SqlServer;

                // SqLite
                //opt.ConnectionString = "Data Source=./Data/SqLiteLocalizations.db";
                // // opt.DbResourceDataManagerType = typeof(DbResourceSqLiteDataManager);    // use this with custom providers
                //opt.DataProvider = DbResourceProviderTypes.SqLite;

                // MySql
                //opt.ConnectionString = "server=localhost;uid=testuser;pwd=super10seekrit;database=Localizations;charset=utf8";
                //opt.DataProvider = DbResourceProviderTypes.MySql;

                opt.ResourceTableName = "localizations";
                //opt.AddMissingResources = false;
                opt.ResxBaseFolder = "~/Properties/";

                // Set up security for Localization Administration form
                opt.ConfigureAuthorizeLocalizationAdministration(actionContext =>
                {
                    // return true or false whether this request is authorized
                    return true;   //actionContext.HttpContext.User.Identity.IsAuthenticated;
                });
            });

            // Optional - Live Reload Middleware
            services.AddLiveReload();

            services.AddMvc()
                .AddViewLocalization()
                .AddDataAnnotationsLocalization()
                .AddDataAnnotationsLocalization(options =>
                {
                    options.DataAnnotationLocalizerProvider = (type, factory) =>
                        factory.Create(typeof(Resources));
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
            ;

            // this *has to go here*  after view localization have been initialized
            // so that Pages can localize - note required even if you're not using
            // the DbResource manager. Fix in post ASP.NET Core 2.0
            services.AddTransient<IViewLocalizer, DbResViewLocalizer>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IConfiguration configuration, IOptions<DbResourceConfiguration> localizationConfig)
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

            var supportedCultures = new[]
            {
                new CultureInfo("en-US"),
                new CultureInfo("en"),
                new CultureInfo("de-DE"),
                new CultureInfo("de"),
                new CultureInfo("fr")
            };
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("en-US"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });

            // Optional - Live Reload Middleware
            // best when run with `dotnet watch run`
            app.UseLiveReload();

            app.UseHttpsRedirection();
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            // print some environment information
            Console.WriteLine("\r\nPlatform: " + System.Runtime.InteropServices.RuntimeInformation.OSDescription);
            Console.WriteLine("DbResourceMode: " + DbResourceConfiguration.Current.ResourceAccessMode);
            Console.WriteLine("Connection: " + DbResourceConfiguration.Current.ConnectionString);
            Console.WriteLine("Connection: " + DbResourceConfiguration.Current.ResourceTableName);
        }
    }
}
