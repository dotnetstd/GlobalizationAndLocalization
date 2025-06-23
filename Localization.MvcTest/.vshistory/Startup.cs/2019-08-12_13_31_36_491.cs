using Localization.MvcTest.Models;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Raveshmand.Localization.Core;
using Raveshmand.Localization.EntityFramework.Extentions;
using Raveshmand.Localization.Json.Extentions;
using Raveshmand.Localization.Xml.Extentions;

namespace Localization.MvcTest
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

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            //Configuring EntityFramework Localizarion
            //var connection = @"Server=localhost;Database=TestDb;user id=sa;password=123qweRt;ConnectRetryCount=0";
            var connection = @"Server=SINJULMSBH\MSSQLSERVER2016;Database=TestLocalizationDb;Trusted_Connection=True;MultipleActiveResultSets=true";
            services.AddDbContextPool<Context>(options => options.UseSqlServer(connection));
            services.AddDbLocalization<Context>(option => { option.CacheDependency = CacheOption.IMemoryCache; });

            //Configuring Xml Localizarion
            services.AddXmlLocalization(options =>
            {
                options.CacheDependency = CacheOption.IDistributedCache;
            });

            //Configuring Json Localizarion
            services.AddJsonLocalization(options =>
            {
                options.CacheDependency = CacheOption.IDistributedCache;
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
