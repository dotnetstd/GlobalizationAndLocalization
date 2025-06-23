using ASPNETCoreAndESearch.Business;

using AspNetCorePagesIdentity.Resources;

using Localization;
using Localization.CustomResourceManager;

using LocalizedRazorPages;

using LocalizerCustom.Models;
using LocalizerCustom.Modelss;
using LocalizerCustom.Override;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;

using Nest;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Threading.Tasks;

using WebApplication1;

namespace LocalizerCustom
{
    public class ClassLibraryLocalizationOptions
    {
        public IReadOnlyDictionary<string, string> ResourcePaths;
    }

    public class Startup
    {
        static string domain; //domain without subdomain

        public Startup()
        {
            domain = Environment.GetEnvironmentVariable("DOMAIN");
        }

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

            services.AddOptions();
            services.Configure<ClassLibraryLocalizationOptions>(
                options => options.ResourcePaths = new Dictionary<string, string>
                {
                    { "HelloServices", "Resources" },
                    { "Localization.CustomResourceManager", "My/Resources" }
                }
            );
            services.TryAddSingleton(typeof(IStringLocalizerFactory), typeof(ClassLibraryStringLocalizerFactory));
            services.AddLocalization();

#if NET461
            supportedCultures.Add(new CultureInfo("zh-CHT"));
#elif NETCOREAPP2_2
#else
#error Target framework needs to be updated
#endif

            services.AddLocalization(options =>
            {
                options.ResourcesPath = "Resources";
            });
            services.AddMvc()
                     .AddViewLocalization();
            services.AddSingleton<ConnectionSettings>(x => new ConnectionSettings(new Uri("http://localhost:9200")));
            services.AddTransient<IProductService, ProductService>();


            services.AddHttpContextAccessor();

            services.AddTransient<ISharedResource, Modelss.SharedResource>();

            services.AddScoped<LanguageActionFilter>();

            services.Configure<RequestLocalizationOptions>(
                options =>
                {
                    var supportedCultures = new List<CultureInfo>
                        {
                            new CultureInfo("en-GB"),
                            new CultureInfo("es-ES"),
                            new CultureInfo("tr-TR"),
                            new CultureInfo("it"),
                            new CultureInfo("en"),
                            new CultureInfo("pt"),
                        };

                    options.DefaultRequestCulture = new RequestCulture(culture: "es-ES", uiCulture: "es-ES");
                    options.SupportedCultures = supportedCultures;
                    options.SupportedUICultures = supportedCultures;
                });

            services.AddLocalization(options => options.ResourcesPath = "Resources");

            /**** Localization configuration ****/
            services.AddSingleton<IdentityLocalizationService>();
            services.AddSingleton<SharedLocalizationService>();
            services.AddSingleton<LocService>();

            services.AddMvc(options =>
                {
                    options.Conventions.Insert(0, new LocalizationConvention());
                    options.Filters.Add(new MiddlewareFilterAttribute(typeof(LocalizationPipeline)));

                    var F = services.BuildServiceProvider().GetService<IStringLocalizerFactory>();
                    var L = F.Create("ModelBindingMessages", "LocalizerCustom");
                    options.ModelBindingMessageProvider.SetValueIsInvalidAccessor(
                        (x) => L["The value '{0}' is invalid."]);
                    options.ModelBindingMessageProvider.SetValueMustBeANumberAccessor(
                        (x) => L["The field {0} must be a number."]);
                    options.ModelBindingMessageProvider.SetMissingBindRequiredValueAccessor(
                        (x) => L["A value for the '{0}' property was not provided.", x]);
                    options.ModelBindingMessageProvider.SetAttemptedValueIsInvalidAccessor(
                        (x, y) => L["The value '{0}' is not valid for {1}.", x, y]);
                    options.ModelBindingMessageProvider.SetMissingKeyOrValueAccessor(
                        () => L["A value is required."]);
                    options.ModelBindingMessageProvider.SetUnknownValueIsInvalidAccessor(
                        (x) => L["The supplied value is invalid for {0}.", x]);
                    options.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor(
                        (x) => L["Null value is invalid.", x]);
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                // Add support for localizing strings in data annotations (e.g. validation messages) via the
                // IStringLocalizer abstractions.
                .AddDataAnnotationsLocalization(options =>
                {
                    options.DataAnnotationLocalizerProvider = (type, factory) =>
                        factory.Create(typeof(Common));

                    options.DataAnnotationLocalizerProvider = (type, factory) =>
                    {
                        var assemblyName = new AssemblyName(typeof(Models.SharedResource).GetTypeInfo().Assembly.FullName);
                        return factory.Create("SharedResource", assemblyName.Name);
                    };

                    options.DataAnnotationLocalizerProvider = (type, factory) =>
                    {
                        var assemblyName = new AssemblyName(typeof(IdentityResource).GetTypeInfo().Assembly.FullName);
                        return factory.Create("IdentityResource", assemblyName.Name);
                    };
                })
                // Add support for finding localized views, based on file name suffix, e.g. Index.fr.cshtml
                .AddViewLocalization(LanguageViewLocationExpanderFormat.SubFolder)
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix, opts => { opts.ResourcesPath = "Resources"; })
            ;

            services.Configure<RouteOptions>(options =>
            {
                options.ConstraintMap.Add("lang", typeof(LanguageRouteConstraint));
                options.ConstraintMap.Add("culturecode", typeof(CultureRouteConstraint));
                options.ConstraintMap.Add("culture", typeof(LanguageRouteConstraint2));
                options.ConstraintMap.Add("culture", typeof(SubdomainRouteConstraint));
            });

            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[] { new CultureInfo("en"), new CultureInfo("fa") };
                options.DefaultRequestCulture = new RequestCulture("en", "en");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });

            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                    new CultureInfo("en-US"),
                    new CultureInfo("tr-TR"),
                };

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

                options.RequestCultureProviders = new[]{ new RouteDataRequestCultureProvider{
                    IndexOfCulture=1,
                    IndexofUICulture=1
                }};
                //options.RequestCultureProviders = new[]{ new QueryStringRequestCultureProvider
                //{
                //    QueryStringKey = "culture",
                //    UIQueryStringKey = "culture"
                //} };


                // You can change which providers are configured to determine the culture for requests, or even add a custom
                // provider with your own logic. The providers will be asked in order to provide a culture for each request,
                // and the first to provide a non-null result that is in the configured supported cultures list will be used.
                // By default, the following built-in providers are configured:
                // - QueryStringRequestCultureProvider, sets culture via "culture" and "ui-culture" query string values, useful for testing
                // - CookieRequestCultureProvider, sets culture via "ASPNET_CULTURE" cookie
                // - AcceptLanguageHeaderRequestCultureProvider, sets culture via the "Accept-Language" request header
                options.RequestCultureProviders.Insert(0, new QueryStringRequestCultureProvider());
                options.RequestCultureProviders.Insert(0, new CookieRequestCultureProvider());

                options.RequestCultureProviders = new[] { new CombinedCultureProvider() };
            });

            services.AddTransient<CustomLocalizer>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
            .AddRazorPagesOptions(options =>
            {
                options.Conventions.Add(new PageCultureConvention());
            });

            services.AddMvc().AddViewLocalization(options => options.ResourcesPath = "Resources");
            // Add patched IViewLocalizer
            services.AddTransient<Microsoft.AspNetCore.Mvc.Localization.IViewLocalizer, ViewLocalizer>();

            services.AddDistributedMemoryCache();
            services.AddSession();
            services.AddMvc(options =>
            {
                options.Filters.Add(new MiddlewareFilterAttribute(typeof(LocalizationPipeline)));
            });

            //services.AddSimpleLocalization<>("");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IStringLocalizer<Startup> startupStringLocalizer)
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

            app.UseSession();
            app.Use(async (context, next) =>
            {
                context.Session.SetString("culture", "ar-YE");
                context.Session.SetString("ui-culture", "ar-YE");
                await next();
            });

            var options = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            LocalizationPipeline.ConfigureOptions(options.Value);
            app.UseRequestLocalization(options.Value);

            var supportedCultures = new[] { new CultureInfo("en"), new CultureInfo("fa") };
            app.UseRequestLocalization(new RequestLocalizationOptions()
            {
                DefaultRequestCulture = new RequestCulture(new CultureInfo("en")),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });

            var supportedCultures22 = new[] { "en-US", "en-AU", "en-GB", "es-ES", "ja-JP", "fr-FR", "zh", "zh-CN" };
            app.UseRequestLocalization(options2 =>
                options2
                    .AddSupportedCultures(supportedCultures22)
                    .AddSupportedUICultures(supportedCultures22)
                    .SetDefaultCulture(supportedCultures22[0])
            // Optionally create an app-specific provider with just a delegate, e.g. look up user preference from DB.
            // Inserting it as position 0 ensures it has priority over any of the default providers.
            //.RequestCultureProviders.Insert(0, new CustomRequestCultureProvider(async context =>
            //{

            //}));
            );


            app.Use((context, next) =>
            {
                if (context.Request.Host.Host.Equals(domain, StringComparison.OrdinalIgnoreCase))
                {
                    var combinedCultureProvider = new CombinedCultureProvider();
                    context.Response.Redirect(combinedCultureProvider.DetermineRedirectURLDueToCulture(context));
                    return Task.FromResult(0);
                }
                return next();
            });

            // Optionally create an app-specific provider with just a delegate, e.g. look up user preference from DB.
            // Inserting it as position 0 ensures it has priority over any of the default providers.
            //options.RequestCultureProviders.Insert(0, new CustomRequestCultureProvider(async context =>
            //{

            //}));

            //app.UseRequestLocalization(options);


            var locOptions = new RequestLocalizationOptions();
            locOptions.SupportedCultures.Add(new CultureInfo("en-US"));
            locOptions.SupportedCultures.Add(new CultureInfo("es-ES"));
            locOptions.SupportedUICultures.Add(new CultureInfo("en-US"));
            locOptions.SupportedUICultures.Add(new CultureInfo("es-ES"));
            locOptions.DefaultRequestCulture = new RequestCulture("en-US", "en-US");
            app.UseRequestLocalization(locOptions);

            app.UseMiddleware<RequestHeaderMiddleware>();

            app.UseRouter(routes =>
            {
                routes.MapMiddlewareRoute("{culture=tr-TR}/{*mvcRoute}", _app =>
                {
                    var supportedCultures2 = new List<CultureInfo>
                    {
                        new CultureInfo("tr-TR"),
                        new CultureInfo("en-US")
                    };

                    var requestLocalizationOptions = new RequestLocalizationOptions
                    {
                        DefaultRequestCulture = new RequestCulture("tr-TR"),
                        SupportedCultures = supportedCultures2,
                        SupportedUICultures = supportedCultures2
                    };

                    requestLocalizationOptions.RequestCultureProviders.Insert(0, new RouteDataRequestCultureProvider());
                    _app.UseRequestLocalization(requestLocalizationOptions);

                    _app.UseMvc(mvcRoutes =>
                    {
                        mvcRoutes.MapRoute(
                            name: "default",
                            template: "{culture=tr-TR}/{controller=Home}/{action=Index}/{id?}");
                    });
                });
            });

            app.Use(async (context, next) =>
            {
                if (context.Request.Path.Value.EndsWith("favicon.ico"))
                {
                    // Pesky browsers
                    context.Response.StatusCode = 404;
                    return;
                }

                context.Response.StatusCode = 200;
                context.Response.ContentType = "text/html; charset=utf-8";

                var requestCultureFeature = context.Features.Get<IRequestCultureFeature>();
                var requestCulture = requestCultureFeature.RequestCulture;

                await context.Response.WriteAsync(
$@"<!doctype html>
<html>
<head>
    <title>{startupStringLocalizer["Request Localization"]}</title>
    <style>
        body {{ font-family: 'Segoe UI', Helvetica, Sans-Serif }}
        h1, h2, h3, h4, th {{ font-family: 'Segoe UI Light', Helvetica, Sans-Serif }}
        th {{ text-align: left }}
    </style>
    <script>
        function useCookie() {{
            var culture = document.getElementById('culture');
            var uiCulture = document.getElementById('uiCulture');
            var cookieValue = '{CookieRequestCultureProvider.DefaultCookieName}=c='+culture.options[culture.selectedIndex].value+'|uic='+uiCulture.options[uiCulture.selectedIndex].value;
            document.cookie = cookieValue;
            window.location = window.location.href.split('?')[0];
        }}

        function clearCookie() {{
            document.cookie='{CookieRequestCultureProvider.DefaultCookieName}=""""';
        }}
    </script>
</head>
<body>");
                await context.Response.WriteAsync($"<h1>ClassLib classLib.SayHello() </h1>");
                await context.Response.WriteAsync($"<h1>Yelling (Outsidenamespace): yellingLib.GetYellingHello()</h1>");
                await context.Response.WriteAsync($"<h1>{startupStringLocalizer["Request Localization Sample"]}</h1>");
                await context.Response.WriteAsync($"<h1>{startupStringLocalizer["Hello"]}</h1>");
                await context.Response.WriteAsync("<form id=\"theForm\" method=\"get\">");
                await context.Response.WriteAsync($"<label for=\"culture\">{startupStringLocalizer["Culture"]}: </label>");
                await context.Response.WriteAsync("<select id=\"culture\" name=\"culture\">");
                await WriteCultureSelectOptions(context);
                await context.Response.WriteAsync("</select><br />");
                await context.Response.WriteAsync($"<label for=\"uiCulture\">{startupStringLocalizer["UI Culture"]}: </label>");
                await context.Response.WriteAsync("<select id=\"uiCulture\" name=\"ui-culture\">");
                await WriteCultureSelectOptions(context);
                await context.Response.WriteAsync("</select><br />");
                await context.Response.WriteAsync("<input type=\"submit\" value=\"go QS\" /> ");
                await context.Response.WriteAsync($"<input type=\"button\" value=\"go cookie\" onclick='useCookie();' /> ");
                await context.Response.WriteAsync($"<a href=\"/\" onclick='clearCookie();'>{startupStringLocalizer["reset"]}</a>");
                await context.Response.WriteAsync("</form>");
                await context.Response.WriteAsync("<br />");
                await context.Response.WriteAsync("<table><tbody>");
                await context.Response.WriteAsync($"<tr><th>Winning provider:</th><td>{requestCultureFeature.Provider?.GetType()?.Name}</td></tr>");
                await context.Response.WriteAsync($"<tr><th>{startupStringLocalizer["Current request culture:"]}</th><td>{requestCulture.Culture.DisplayName} ({requestCulture.Culture})</td></tr>");
                await context.Response.WriteAsync($"<tr><th>{startupStringLocalizer["Current request UI culture:"]}</th><td>{requestCulture.UICulture.DisplayName} ({requestCulture.UICulture})</td></tr>");
                await context.Response.WriteAsync($"<tr><th>{startupStringLocalizer["Current thread culture:"]}</th><td>{CultureInfo.CurrentCulture.DisplayName} ({CultureInfo.CurrentCulture})</td></tr>");
                await context.Response.WriteAsync($"<tr><th>{startupStringLocalizer["Current thread UI culture:"]}</th><td>{CultureInfo.CurrentUICulture.DisplayName} ({CultureInfo.CurrentUICulture})</td></tr>");
                await context.Response.WriteAsync($"<tr><th>{startupStringLocalizer["Current date (invariant full):"]}</th><td>{DateTime.Now.ToString("F", CultureInfo.InvariantCulture)}</td></tr>");
                await context.Response.WriteAsync($"<tr><th>{startupStringLocalizer["Current date (invariant):"]}</th><td>{DateTime.Now.ToString(CultureInfo.InvariantCulture)}</td></tr>");
                await context.Response.WriteAsync($"<tr><th>{startupStringLocalizer["Current date (request full):"]}</th><td>{DateTime.Now.ToString("F")}</td></tr>");
                await context.Response.WriteAsync($"<tr><th>{startupStringLocalizer["Current date (request):"]}</th><td>{DateTime.Now.ToString()}</td></tr>");
                await context.Response.WriteAsync($"<tr><th>{startupStringLocalizer["Current time (invariant):"]}</th><td>{DateTime.Now.ToString("T", CultureInfo.InvariantCulture)}</td></tr>");
                await context.Response.WriteAsync($"<tr><th>{startupStringLocalizer["Current time (request):"]}</th><td>{DateTime.Now.ToString("T")}</td></tr>");
                await context.Response.WriteAsync($"<tr><th>{startupStringLocalizer["Big number (invariant):"]}</th><td>{(Math.Pow(2, 42) + 0.42).ToString("N", CultureInfo.InvariantCulture)}</td></tr>");
                await context.Response.WriteAsync($"<tr><th>{startupStringLocalizer["Big number (request):"]}</th><td>{(Math.Pow(2, 42) + 0.42).ToString("N")}</td></tr>");
                await context.Response.WriteAsync($"<tr><th>{startupStringLocalizer["Big number negative (invariant):"]}</th><td>{(-Math.Pow(2, 42) + 0.42).ToString("N", CultureInfo.InvariantCulture)}</td></tr>");
                await context.Response.WriteAsync($"<tr><th>{startupStringLocalizer["Big number negative (request):"]}</th><td>{(-Math.Pow(2, 42) + 0.42).ToString("N")}</td></tr>");
                await context.Response.WriteAsync($"<tr><th>{startupStringLocalizer["Money (invariant):"]}</th><td>{2199.50.ToString("C", CultureInfo.InvariantCulture)}</td></tr>");
                await context.Response.WriteAsync($"<tr><th>{startupStringLocalizer["Money (request):"]}</th><td>{2199.50.ToString("C")}</td></tr>");
                await context.Response.WriteAsync($"<tr><th>{startupStringLocalizer["Money negative (invariant):"]}</th><td>{(-2199.50).ToString("C", CultureInfo.InvariantCulture)}</td></tr>");
                await context.Response.WriteAsync($"<tr><th>{startupStringLocalizer["Money negative (request):"]}</th><td>{(-2199.50).ToString("C")}</td></tr>");
                await context.Response.WriteAsync("</tbody></table>");
                await context.Response.WriteAsync(
@"</body>
</html>");
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "LocalizedDefault",
                    template: "{lang:lang}/{controller=Home}/{action=Index}/{id?}"
                );

                routes.MapRoute(
                    name: "default",
                    template: "{*catchall}",
                    defaults: new { controller = "Home", action = "Index", lang = "en" }); /*RedirectToDefaultLanguage*/

                routes.MapRoute(
                    name: "default",
                    template: "{culture:culturecode}/{controller=Home}/{action=Index}/{id?}");


                routes.MapRoute(
                    name: "CustomRouterWithCulture",
                    template: "{culture::regex(^[a-z]{{2}}$)}/{slug1}/{slug2?}",
                    defaults: new { controller = "Home", action = "Router" });

                routes.MapRoute(
                     name: "CustomRouter",
                     template: "{slug1}/{slug2?}", // ... goes depends on your slug level
                     defaults: new { controller = "Home", action = "Router" });


                routes.MapRoute(
                        name: "About",
                        template: "about",
                        defaults: new { controller = "Home", action = "About" },
                        constraints: new { culture = new SubdomainRouteConstraint() }
                );
                routes.MapRoute(
                        name: "Contact",
                        template: "contact",
                        defaults: new { controller = "Home", action = "Contact" },
                        constraints: new { culture = new SubdomainRouteConstraint() }
                );
                routes.MapRoute(
                        name: "LocalizedDefault",
                        template: "{controller}/{action}/{id?}",
                        defaults: new { controller = "Home", action = "Index", culture = "en" },
                        constraints: new { culture = new SubdomainRouteConstraint() }
                );
                routes.MapRoute(
                      name: "default",
                      template: "{*catchall}",
                      defaults: new { controller = "Home", action = "RedirectToDefaultCulture", culture = "en" });

                //routes.MapGet("{culture:culturecode}/{*path}", appBuilder => { });

                //routes.MapGet("{*path}", (RequestDelegate)(ctx =>
                //{
                //    var defaultCulture = localizationOptions.DefaultRequestCulture.Culture.Name;
                //    var path = ctx.GetRouteValue("path") ?? string.Empty;
                //    var culturedPath = $"/{defaultCulture}/{path}";
                //    ctx.Response.Redirect(culturedPath);
                //    return Task.CompletedTask;
                //}));

                routes.MapRoute(
                        name: "LocalizedDefault",
                        template: "{culture:culture}/{controller=Home}/{action=Index}/{id?}"
                );
                routes.MapRoute(
                      name: "default",
                      template: "{*catchall}",
                      defaults: new { controller = "Home", action = "RedirectToDefaultCulture", culture = "en" });

                routes.MapRoute(
                    name: "default",
                    template: "{culture}/{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private static async System.Threading.Tasks.Task WriteCultureSelectOptions(HttpContext context)
        {
            await context.Response.WriteAsync($"    <option value=\"\">-- select --</option>");
            await context.Response.WriteAsync($"    <option value=\"{new CultureInfo("en-US").Name}\">{new CultureInfo("en-US").DisplayName}</option>");
            await context.Response.WriteAsync($"    <option value=\"{new CultureInfo("en-AU").Name}\">{new CultureInfo("en-AU").DisplayName}</option>");
            await context.Response.WriteAsync($"    <option value=\"{new CultureInfo("en-GB").Name}\">{new CultureInfo("en-GB").DisplayName}</option>");
            await context.Response.WriteAsync($"    <option value=\"{new CultureInfo("fr-FR").Name}\">{new CultureInfo("fr-FR").DisplayName}</option>");
            await context.Response.WriteAsync($"    <option value=\"{new CultureInfo("es-ES").Name}\">{new CultureInfo("es-ES").DisplayName}</option>");
            await context.Response.WriteAsync($"    <option value=\"{new CultureInfo("ja-JP").Name}\">{new CultureInfo("ja-JP").DisplayName}</option>");
            await context.Response.WriteAsync($"    <option value=\"{new CultureInfo("zh").Name}\">{new CultureInfo("zh").DisplayName}</option>");
            await context.Response.WriteAsync($"    <option value=\"{new CultureInfo("zh-CN").Name}\">{new CultureInfo("zh-CN").DisplayName}</option>");
#if NET461
            await context.Response.WriteAsync($"    <option value=\"{new CultureInfo("zh-CHT").Name}\">{new CultureInfo("zh-CHT").DisplayName}</option>");
#elif NETCOREAPP2_2
#else
#error Target framework needs to be updated
#endif
            await context.Response.WriteAsync($"    <option value=\"en-NOTREAL\">English (Not a real locale)</option>");
            await context.Response.WriteAsync($"    <option value=\"pp-NOTREAL\">Made-up (Not a real anything)</option>");
        }

    }
}
