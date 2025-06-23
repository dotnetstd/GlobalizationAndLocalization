using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Localization.Phema
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

            // Add
            //services.AddPhemaLocalization(configuration =>
            //    configuration.AddCulture(CultureInfo.GetCultureInfo("en", culture =>
            //        culture.AddComponent<Model, IModelLocalizationComponent, EnglishModelLocalizationComponent>()),
            //    configuration.AddCulture(CultureInfo.GetCultureInfo("ru"), culture =>
            //        culture.AddComponent<Model, IModelLocalizationComponent, RussianModelLocalizationComponent>())));

            //Phema.Localization.Cultures
            //Set of extension methods for culture configuration
            //services.AddPhemaLocalization(configuration => configuration.AddEnglishCulture(culture => { }));

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

            //// Configure
            //app.UseRequestLocalization();
            //// Get
            //var localizer = serviceProvider.GetRequiredService<ILocalizer>();

            //// Use
            //var message = localizer.Localize<IModelLocalizationComponent>(c => c.SomeMessage);

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
