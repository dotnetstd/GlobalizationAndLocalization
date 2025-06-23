using DbLocalizationProvider.AdminUI.AspNetCore;
using DbLocalizationProvider.AspNetCore;
using DbLocalizationProvider.Core.AspNetSample.Data;
using DbLocalizationProvider.Core.AspNetSample.Models;
using DbLocalizationProvider.Core.AspNetSample.Resources;
using DbLocalizationProvider.Core.AspNetSample.Services;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using System.Collections.Generic;
using System.Globalization;

namespace DbLocalizationProvider.Core.AspNetSample
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
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                    .AddEntityFrameworkStores<ApplicationDbContext>()
                    .AddDefaultTokenProviders();

            services.AddTransient<IEmailSender, EmailSender>();

            services.AddMvc()
                    .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                    .AddViewLocalization()
                    .AddDataAnnotationsLocalization();

            services.Configure<RequestLocalizationOptions>(opts =>
                                                           {
                                                               var supportedCultures = new List<CultureInfo>
                                                                                       {
                                                                                           new CultureInfo("en"),
                                                                                           new CultureInfo("no")
                                                                                       };

                                                               opts.DefaultRequestCulture = new RequestCulture("en");
                                                               opts.SupportedCultures = supportedCultures;
                                                               opts.SupportedUICultures = supportedCultures;
                                                           });

            services.AddDbLocalizationProvider(_ =>
                                               {
                                                   //CacheManager	Gets or sets cache manager used to store resources and translations (InMemory by default)
                                                   //Connection	Gets or sets the name of the database connection (e.g. "DefaultConnection").
                                                   //CustomAttributes	Gets or sets list of custom attributes that should be discovered and registered during startup scanning.
                                                   //DefaultResourceCulture	Gets or sets the default resource culture to register translations for newly discovered resources.
                                                   //DiagnosticsEnabled	Gets or sets value enabling or disabling diagnostics for localization provider (e.g. missing keys will be written to log file).
                                                   //DiscoverAndRegisterResources	Gets or sets the flag to control localized models discovery and registration during app startup.
                                                   //EnableInvariantCultureFallback	Gets or sets flag to enable or disable invariant culture fallback (to use resource values discovered & registered from code).
                                                   //EnableLocalization	Gets or sets the callback function for enabling or disabling localization. If this returns false - requested resource key will be returned as translation.
                                                   //Export	Gets or sets settings used for export of the resources.
                                                   //ForeignResources	Gets or sets collection of foreign resources. Foreign resource descriptors are used to include classes without [LocalizedResource] or [LocalizedModel] attributes.
                                                   //Import	Gets or sets settings to be used during resource import.
                                                   //ModelMetadataProviders	Settings for model metadata providers.
                                                   //PopulateCacheOnStartup	Gets or sets a value indicating whether cache should be populated during startup (default = true).
                                                   //ScanAllAssemblies	Forces type scanner to load all referenced assemblies. When enabled, scanner is not relying on current AppDomain.GetAssemblies but checks referenced assemblies recursively (default false).
                                                   //TypeFactory	Returns type factory used internally for creating new services or handlers for commands.
                                                   //TypeScanners	Gets list of all known type scanners.
                                                   _.EnableInvariantCultureFallback = true;
                                                   _.CustomAttributes.Add(typeof(WeirdCustomAttribute));
                                                   _.Connection = "DefaultConnection";
                                                   _.ScanAllAssemblies = true;
                                                   _.ModelMetadataProviders = new ModelMetadataProvidersConfiguration()
                                                   {
                                                       //MarkRequiredFields   Set true to add translation returned from RequiredFieldResource for required fields.
                                                       //ReplaceProviders    Gets or sets a value to replace ModelMetadataProvider to use new db localization system.
                                                       //RequiredFieldResource   If MarkRequiredFields is set to true, return of this method will be used to indicate required fields(added at the end of label).
                                                       //UseCachedProviders Gets or sets a value to use cached version of ModelMetadataProvider.
                                                   };
                                               });
            services.AddDbLocalizationProviderAdminUI(_ =>
                                                      {
                                                          _.RootUrl = "/localization-admin";
                                                          _.AuthorizedAdminRoles.Add("Admin");
                                                          _.ShowInvariantCulture = true;
                                                          _.ShowHiddenResources = false;
                                                          _.DefaultView = ResourceListView.Tree;
                                                          _.CustomCssPath = "/css/custom-adminui.css";
                                                      });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
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

            var options = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(options.Value);

            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseMvc(routes =>
                       {
                           routes.MapRoute(
                                           name: "default",
                                           template: "{controller=Home}/{action=Index}/{id?}");
                       });

            app.UseDbLocalizationProvider();
            app.UseDbLocalizationClientsideProvider(path: "/jsl10n");
            app.UseDbLocalizationProviderAdminUI();
        }
    }
}
