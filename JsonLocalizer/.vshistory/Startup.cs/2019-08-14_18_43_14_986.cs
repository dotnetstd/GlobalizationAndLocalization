using Anvyl.JsonLocalizer;

using JsonLocalizer.CookieLocalizationApp;

using LocalizationCore;

using LocalizationSample;
using LocalizationSample.Resources;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;

using My.AspNetCore.Localization.Json;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

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

            //if (Environment.IsProduction())
            //{
            //	services.AddDistributedRedisCache(opts =>
            //	{
            //		opts.Configuration = "localhost";
            //		opts.InstanceName = "JsonLocalizer";
            //	});
            //}

            if (Environment.IsDevelopment())
                services.AddDistributedMemoryCache();

            services.Configure<JsonLocalizerOptions>(Configuration.GetSection(nameof(JsonLocalizerOptions)));
            services.AddSingleton<IStringLocalizerFactory, JsonStringLocalizerFactory>();

            // Add localization based on JSON files.
            services.AddJsonLocalization2(options => options.ResourcesPath = "Resources");

            //services
            //    // Adds JSON localization
            //    .AddJsonLocalization(options => options.ResourcesPath = "Resources")
            //    // Add MVC as usual
            //    .AddMvc()
            //    //Add localization provider for Views
            //    .AddViewLocalization();

            //services.AddJsonLocalization(opts =>
            //{
            //    opts.ResourcesPath = "Resources";
            //});

            services.AddJsonLocalization(options =>
            {
                options.ResourcesPath = "Resources";
                options.GlobalResourceFileName = "global";
                options.AreasResourcePrefix = "areas";
            });

            services.AddSession();
            services.AddMvcLocalization("zh", new string[] { "zh-CN", "ja-JP", "en" });
            services.AddCodeMatching();

            services.AddSingleton<IPageFactoryProvider, LocalizationFixPageFactoryProvider>();
            services.AddMvc().AddViewLocalization();

            //services.AddJsonLocalization(options => options.ResourcesPath = "Resources");
            services.AddMvc().AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
               .AddDataAnnotationsLocalization();
            //services.AddScoped<LanguageActionFilter>();
            services.Configure<RequestLocalizationOptions>(
            options =>
            {
                var supportedCultures = new List<CultureInfo>
                {
                                new CultureInfo("en-US"),
                                new CultureInfo("vn-VN"),
                };

                options.DefaultRequestCulture = new RequestCulture(culture: "vn-VN", uiCulture: "vn-VN");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
                options.RequestCultureProviders = new[]
                {
                                new RouteDataRequestCultureProvider
                                {
                                    IndexOfCulture=1,
                                    IndexofUICulture=1
                                }
                };
            });


            services.Configure<RouteOptions>(options =>
            {
                options.ConstraintMap.Add("culture", typeof(LanguageRouteConstraint));
            });

            services.AddMvc()
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
            ;

            //services.AddJsonLocalization(options => options.ResourcesPath = "Resources");


            //services.AddJsonLocalization(options => {
            //    options.CacheDuration = TimeSpan.FromMinutes(15);
            //    options.ResourcesPath = "mypath";
            //    options.FileEncoding = Encoding.GetEncoding("ISO-8859-1");
            //    options.SupportedCultureInfos = new HashSet<CultureInfo>()
            //    {
            //      new CultureInfo("en-US"),
            //      new CultureInfo("fr-FR")
            //    };
            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app,
            IHostingEnvironment env,
            IOptions<RequestLocalizationOptions> localizationOptions,
            IStringLocalizer<Startup> localizer1,
            IStringLocalizer<Model> localizer2)
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
                    new CultureInfo("ro-RO"),
                    new CultureInfo("en-GB"),
                    new CultureInfo("zh-TW")
                },
                SupportedUICultures = new CultureInfo[]{
                    new CultureInfo("en-US"),
                    new CultureInfo("ro-RO"),
                    new CultureInfo("en-GB"),
                    new CultureInfo("zh-TW")
                }
            };

            options.RequestCultureProviders.Insert(0, new CookieRequestCultureProvider());
            var optionss = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(options);
            //app.UseRequestLocalization(optionss.Value);


            // Localization
            app.UseRequestLocalization(localizationOptions.Value);
            app.UseJsonLocalizer();

            app.UseSession();
            app.UseMvcLocalization(true);

            options.RequestCultureProviders.Insert(0, new JsonRequestCultureProvider());
            app.UseRequestLocalization(options);

            //var supportedCultures = new List<CultureInfo>
            //{
            //    new CultureInfo("en-US"),
            //    new CultureInfo("fr-FR")
            //};
            //var options = new RequestLocalizationOptions
            //{
            //    DefaultRequestCulture = new RequestCulture("en-US"),
            //    SupportedCultures = supportedCultures,
            //    SupportedUICultures = supportedCultures
            //};
            //app.UseRequestLocalization(options);
            app.Run(async (context) =>
            {
                await context.Response.WriteAsync($"{localizer1["Hello"]} - {localizer2["Hello"]}!!");
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            // Call the extension method (that extends IApplicationBuilder) to provide cookie localization. //
            app.UseCookieLocalization();
            // *******

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapAreaRoute(
                    name: "Admin",
                    areaName: "Admin",
                    template: "/Admin/{controller=Home}/{action=Index}/{id?}");
            });
        }

        public class LanguageRouteConstraint : IRouteConstraint
        {
            public bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
            {
                if (!values.ContainsKey("culture"))
                    return false;

                var culture = values["culture"].ToString();
                return culture == "en-US" || culture == "vn-VN";
            }
        }

        public class RouteDataRequestCultureProvider : RequestCultureProvider
        {
            public int IndexOfCulture;
            public int IndexofUICulture;
            public override Task<ProviderCultureResult> DetermineProviderCultureResult(HttpContext httpContext)
            {
                if (httpContext == null)
                    throw new ArgumentNullException(nameof(httpContext));
                string culture = null;
                string uiCulture = null;
                var twoLetterCultureName = httpContext.Request.Path.Value.Split('/')[IndexOfCulture]?.ToString();
                var twoLetterUICultureName = httpContext.Request.Path.Value.Split('/')[IndexofUICulture]?.ToString();
                if (twoLetterCultureName == "vn")
                    culture = "vn-VN";
                else if (twoLetterCultureName == "en")
                    culture = uiCulture = "en-US";
                if (twoLetterUICultureName == "vn")
                    culture = "vn-VN";
                else if (twoLetterUICultureName == "en")
                    culture = uiCulture = "en-US";
                if (culture == null && uiCulture == null)
                    return NullProviderCultureResult;

                if (culture != null && uiCulture == null)
                    uiCulture = culture;
                if (culture == null && uiCulture != null)
                    culture = uiCulture;
                var providerResultCulture = new ProviderCultureResult(culture, uiCulture);
                return Task.FromResult(providerResultCulture);
            }
        }
    }
}
