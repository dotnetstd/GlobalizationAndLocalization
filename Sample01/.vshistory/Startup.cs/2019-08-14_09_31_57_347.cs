
using MainApp.Models.Localization;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;

using OrchardCore.Localization;

using Sample01.Data;
using Sample01.Localization;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Resources;

//[assembly: ResourceLocation("Resource Folder Name")]
//[assembly: RootNamespace("App Root Namespace")]
[assembly: NeutralResourcesLanguage("fa-IR")]

namespace Sample01
{
    public class Startup
    {
        private const string faCulture = "fa";
        private const string faUICulture = "fa-IR";
        private const string enUSCulture = "en";
        private const string enUSUICulture = "en-US";

        public static string UiCulture;
        public static string CultureDirection;
        public static IStringLocalizer _e; // This is how we access language strings
        public static IConfiguration LocalConfig;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            LocalConfig = Configuration;

            // Returns "Welcome" for en-US and "خوش آمدید" for fa-IR
            var welcome = Startup._e["Welcome"];
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

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<IdentityUser>()
                .AddDefaultUI(UIFramework.Bootstrap4)
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            //services.AddSingleton<IPageApplicationModelProvider, CustomHandlerPageApplicationModelProvider>();

            #region ◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘ Localization ◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘
            //services.AddScoped<ITagHelperComponent, LanguageDirectionTagHelperComponent>();
            //services.AddSingleton<IPageFactoryProvider, LocalizationFixPageFactoryProvider>();
            //services.AddMvc().AddViewLocalization();
            services.AddSession();
            services.AddJsonLocalization(options => options.ResourcesPath = "Resources");
            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.AddMvc(options =>
            {
                //options.Filters.Add(new MiddlewareFilterAttribute(typeof(LocalizationPipeline)));
            })
            .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
            .AddDataAnnotationsLocalization();


            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                    new CultureInfo(enUSUICulture),
                    new CultureInfo(faUICulture),
                };

                options.DefaultRequestCulture = new RequestCulture(culture: enUSCulture, uiCulture: enUSUICulture);

                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;



                //var provider = new RouteDataRequestCultureProvider();
                //provider.RouteDataStringKey = "lang";
                //provider.UIRouteDataStringKey = "lang";
                //provider.Options = options;s
                //options.RequestCultureProviders = new[] { provider };

                //ASP.NET Core localization APIs have four default providers that can determine the current culture of an executing request:
                //options.RequestCultureProviders.Add(new QueryStringRequestCultureProvider());
                //options.RequestCultureProviders.Add(new CookieRequestCultureProvider());
                //options.RequestCultureProviders.Add(new AcceptLanguageHeaderRequestCultureProvider());
                //options.RequestCultureProviders.Insert(0, new CustomRequestCultureProvider(async context =>
                //{
                //    return new ProviderCultureResult("en");
                //}));

                options.RequestCultureProviders.Add(new AppSettingsRequestCultureProvider());

                //options.AddInitialRequestCultureProvider(
                //    new CustomRequestCultureProvider(async context =>
                //    {
                //        var currentCulture = "en";
                //        var segments = context.Request.Path.Value.Split(new char[] { '/' },StringSplitOptions.RemoveEmptyEntries);

                //        if (segments.Length > 1 && segments[0].Length == 2)
                //        {
                //            currentCulture = segments[0];
                //        }

                //        var requestCulture = new ProviderCultureResult(culture);

                //        return Task.FromResult(requestCulture);
                //    })
                //);

            });

            services.Configure<RouteOptions>(options =>
            {
                options.ConstraintMap.Add("lang", typeof(LanguageRouteConstraint));
            });

            services.AddLocalization(options => options.ResourcesPath = "Resources");

            services.AddMvc()
            .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
            .AddDataAnnotationsLocalization(options =>
            {
                options.DataAnnotationLocalizerProvider = (type, factory) =>
                    factory.Create(typeof(MainApp.Models.Localization.SharedResource));
            });


            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
             {
                              new CultureInfo(faCulture),
                              new CultureInfo(faUICulture),
                              new CultureInfo(enUSCulture),
                              new CultureInfo(enUSUICulture),
                      };
                options.DefaultRequestCulture = new RequestCulture(culture: enUSCulture, uiCulture: enUSUICulture);
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });

            //services.AddOrchardCms();
            services.AddSingleton<ILocalizationFileLocationProvider, MyLIPoFileLocationProvider>();

            //var cookieProvider = localizationOptions.RequestCultureProviders
            //                          .OfType<CookieRequestCultureProvider>()
            //                                 .First();

            //cookieProvider.CookieName = "UserCulture";

            //routes www.mysite.com/en-us/Home/Index or www.mysite.com/fi-fi/Home/Index?

            //var requestProvider = new RouteDataRequestCultureProvider();
            //localizationOptions.RequestCultureProviders.Insert ( 0 , requestProvider );

            //app.UseRouter ( routes =>
            //{
            //      routes.MapMiddlewareRoute ( "{culture=en-US}/{*mvcRoute}" , subApp =>
            //      {
            //            subApp.UseRequestLocalization ( localizationOptions );

            //            subApp.UseMvc ( mvcRoutes =>
            //            {
            //                  mvcRoutes.MapRoute (
            //                      name: "default" ,
            //                      template: "{culture=en-US}/{controller=Home}/{action=Index}/{id?}" );
            //            } );
            //      } );
            //} );


            services.AddMvc()
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix);

            services.AddPortableObjectLocalization();
            //services.AddPortableObjectLocalization(options => options.ResourcesPath = "Localization");
            // Override the default localization file locations with Orchard specific ones
            //services.Replace(ServiceDescriptor.Singleton<ILocalizationFileLocationProvider, ModularPoFileLocationProvider>());


            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new List<CultureInfo>
                {
                    new CultureInfo("en-US"),
                    new CultureInfo("en"),
                    new CultureInfo("fr-FR"),
                    new CultureInfo("fr"),
                    new CultureInfo("cs"),
                };

                options.DefaultRequestCulture = new RequestCulture("en-US");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });


            #region snippet1
            services.AddLocalization(options => options.ResourcesPath = "Resources");

            services.AddMvc()
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization();
            #endregion

            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new List<CultureInfo>
                    {
                        new CultureInfo("en-US"),
                        new CultureInfo("fr")
                    };

                options.DefaultRequestCulture = new RequestCulture("en-US");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });

            #endregion
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "localized",
                    template: "{culture=en}/{controller=Home}/{action=Index}");
            });

            #region ◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘  Localization  ◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘


            var cultures = new List<CultureInfo>
            {
                new CultureInfo("en-GB"),
                new CultureInfo("th-TH")
            };

            var requestCulture = new RequestCulture("en-GB", "en-GB");

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = requestCulture,
                SupportedCultures = cultures,
                SupportedUICultures = cultures,
                RequestCultureProviders = new List<IRequestCultureProvider>
                {
                    new CookieRequestCultureProvider
                    {
                        CookieName = "Web.Language"
                    }
                }
            });

            //var supportedCultures = new[]
            //{
            //    new CultureInfo("en-US"),
            //    new CultureInfo("ar-YE")
            //};

            //var options = new RequestLocalizationOptions {
            //    DefaultRequestCulture = new RequestCulture("en-US"),
            //    SupportedCultures = supportedCultures,
            //    SupportedUICultures = supportedCultures
            //};

            //options.RequestCultureProviders.Insert(0, new JsonRequestCultureProvider());
            //app.UseRequestLocalization(options);
            var supportedCultures = new List<CultureInfo>
            {
                new CultureInfo("en-US"),
                new CultureInfo("fa-IR"),
                new CultureInfo("fr-FR")
            };
            var options = new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("en-US", "en-US"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            };
            options.RequestCultureProviders = new[]
            {
                new SessionStateRequestCultureProvider() { Options = options }
            };
            app.UseRequestLocalization(options);
            app.UseSession();
            app.Use(async (context, next) =>
            {
                context.Session.SetString("culture", "ar-YE");
                context.Session.SetString("ui-culture", "ar-YE");
                await next();
            });

            var localizationOption = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(localizationOption.Value);

            // a list of all available languages
            //var supportedCultures = new List<CultureInfo>
            //{
            //    new CultureInfo("en-US"),
            //    new CultureInfo("fa-IR")
            //};

            var requestLocalizationOptions = new RequestLocalizationOptions
            {
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures,
            };
            requestLocalizationOptions.RequestCultureProviders.Insert(0, new JsonRequestCultureProvider());
            app.UseRequestLocalization(requestLocalizationOptions);




            var localizationOptions = new RequestLocalizationOptions
            {
                SupportedCultures = new List<CultureInfo>
                {
                    new CultureInfo("de-DE"),
                    new CultureInfo("en-US"),
                    new CultureInfo("en-GB")
                },
                SupportedUICultures = new List<CultureInfo>
                {
                    new CultureInfo("de-DE"),
                    new CultureInfo("en-US"),
                    new CultureInfo("en-GB")
                },
                DefaultRequestCulture = new RequestCulture("en-US")
            };

            // Adding our UrlRequestCultureProvider as first object in the list
            localizationOptions.RequestCultureProviders.Insert(0, new UrlRequestCultureProvider
            {
                Options = localizationOptions
            });

            app.UseRequestLocalization(localizationOptions);

            app.UseRequestLocalization();


            var supportedCultures3 = new[]
            {
                new CultureInfo("en-US"),
                new CultureInfo("fr"),
            };

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("en-US"),
                // Formatting numbers, dates, etc.
                SupportedCultures = supportedCultures,
                // UI strings that we have localized.
                SupportedUICultures = supportedCultures
            });

            app.Run(async (context) =>
            {
                context.Response.StatusCode = 200;
                context.Response.ContentType = "text/html;charset=utf-8";

                var detectedCultureName = CultureInfo.CurrentCulture.DisplayName;
                var detectedUICulture = CultureInfo.CurrentUICulture.DisplayName;

                var cultureTable = "<html><body>"
                + "<table border=\"1\">"
                + $"<tr><td>Deducted Culture</td><td>{detectedCultureName}</td></tr>"
                + $"<tr><td>Deducted UI Culture</td><td>{detectedUICulture}</td></tr>"
                + $"<tr><td>Today's Date</td><td>{DateTime.Now:D}</td></tr>"
                + $"<tr><td>Culture's Formatted Number</td><td>{(1234567.89):n}</td></tr>"
                + $"<tr><td>Culture's Currency</td><td>{(42):c}</td></tr>"
                + $"</body></html>";

                await context.Response.WriteAsync(cultureTable);
            });

            var supportedCulturess = new[]
            {
                new CultureInfo("fi"),
                new CultureInfo("no"),
                new CultureInfo("se")
            };

            var requestLocalizationOptionss = new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("se"),
                SupportedCultures = supportedCulturess,
                SupportedUICultures = supportedCulturess
            };

            if (!env.IsDevelopment())
            {
                requestLocalizationOptions.RequestCultureProviders.Insert(0, new TopLevelDomainRequestCultureProvider());
            }

            app.UseRequestLocalization(requestLocalizationOptionss);

            #endregion
        }
    }
}
