using LocalizerCustom.Models;
using LocalizerCustom.Modelss;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;

using System.Collections.Generic;
using System.Globalization;

using WebApplication1;

namespace LocalizerCustom
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
                            new CultureInfo("it")
                        };

                    options.DefaultRequestCulture = new RequestCulture(culture: "es-ES", uiCulture: "es-ES");
                    options.SupportedCultures = supportedCultures;
                    options.SupportedUICultures = supportedCultures;
                });

            services.AddLocalization(options => options.ResourcesPath = "Resources");

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
                .AddDataAnnotationsLocalization(options =>
                {
                    options.DataAnnotationLocalizerProvider = (type, factory) =>
                        factory.Create(typeof(Common));
                })
                .AddViewLocalization(LanguageViewLocationExpanderFormat.SubFolder)
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix, opts => { opts.ResourcesPath = "Resources"; })
            ;

            services.Configure<RouteOptions>(options =>
            {
                options.ConstraintMap.Add("lang", typeof(LanguageRouteConstraint));
                options.ConstraintMap.Add("culturecode", typeof(CultureRouteConstraint));
            });

            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[] { new CultureInfo("en"), new CultureInfo("fa") };
                options.DefaultRequestCulture = new RequestCulture("en", "en");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });

            services.AddTransient<CustomLocalizer>();
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

            var options = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            LocalizationPipeline.ConfigureOptions(options.Value);
            app.UseRequestLocalization(options.Value);

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
                    name: "default",
                    template: "{culture}/{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
