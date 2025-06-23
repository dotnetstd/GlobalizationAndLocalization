using CustomLocalizer;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;

namespace Internationalization
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

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddMyLocalization();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app,
            IHostingEnvironment env,
            IStringLocalizer<Startup> stringLocalizer,
            IHttpContextAccessor contextAccessor)
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

            app.UseRequestLocalization(BuildLocalizationOptions());

            app.Use(async (context, next) =>
            {
                context.Response.StatusCode = 200;
                context.Response.ContentType = "text/html; charset=utf-8";

                await context.Response.WriteAsync(BuildResponse(stringLocalizer));
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync(BuildResponse(contextAccessor));
            });
        }

        private RequestLocalizationOptions BuildLocalizationOptions()
        {
            var supportedCultures = new List<CultureInfo>
            {
                new CultureInfo("en-US"),
                new CultureInfo("it-IT"),
                new CultureInfo("ja-JP"),
                new CultureInfo("nl-NL"),
                new CultureInfo("ru-RU"),
                new CultureInfo("sv-SE")
            };

            return new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("en-US"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            };
        }

        private string BuildResponse(IStringLocalizer stringLocalizer)
        {
            var currentCultureName = CultureInfo.CurrentCulture.EnglishName;
            var currentUICultureName = CultureInfo.CurrentUICulture.EnglishName;

            return "<html><body>"
                + $"<h2>{stringLocalizer["Hello"]}!</h2><table border=\"1\" cellpadding=\"5\" style=\"border-collapse:collapse;\">"
                + $"<tr><td>{stringLocalizer["Current Culture"]}</td><td>{currentCultureName}</td></tr>"
                + $"<tr><td>{stringLocalizer["Current UI Culture"]}</td><td>{currentUICultureName}</td></tr>"
                + $"<tr><td>{stringLocalizer["The Current Date"]}</td><td>{DateTime.Now.ToString("D")}</td></tr>"
                + $"<tr><td>{stringLocalizer["A Formatted Number"]}</td><td>{(1234567.89).ToString("n")}</td></tr>"
                + $"<tr><td>{stringLocalizer["A Currency Value"]}</td><td>{(42).ToString("C")}</td></tr></table>"
                + $"<h2>{stringLocalizer["Goodbye"]}</h2>"
                + "</body></html>";
        }

        //Geolocation
        private string BuildResponse(IHttpContextAccessor contextAccessor)
        {
            String UserIP = contextAccessor.HttpContext.Connection.LocalIpAddress.ToString();
            if (string.IsNullOrEmpty(UserIP))
            {
                UserIP = contextAccessor.HttpContext.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress?.ToString();
            }
            if (String.Compare(UserIP, "::1") == 0)
            {
                UserIP = "ENTER A VALID IP ADDRESS HERE";
            }
            string url = "http://freegeoip.net/json/" + UserIP;
            HttpClient client = new HttpClient();
            Task<string> jsonstring = GetGeoAsync(client, url);
            jsonstring.Wait();
            var dynObj = JsonConvert.DeserializeObject<GeoIP>(jsonstring.Result);

            return "<html><body>" +
                "<table border=\"1\" cellpadding=\"5\" style=\"border-collapse:collapse;\">" +
                $"<tr><td>IP</td><td>{dynObj.ip}</td></tr>" +
                $"<tr><td>Country</td><td>{dynObj.country_name}</td></tr>" +
                $"<tr><td>Region</td><td>{dynObj.region_name}</td></tr>" +
                $"<tr><td>Timezone</td><td>{dynObj.time_zone}</td></tr>" +
                $"<tr><td>Laitude</td><td>{dynObj.latitude}</td></tr>" +
                $"<tr><td>Longitude</td><td>{dynObj.longitude}</td></tr>" +
                "</table></body></html>";
        }
        private async Task<String> GetGeoAsync(HttpClient client, string url)
        {
            String geolocation = null;
            HttpResponseMessage response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                geolocation = await response.Content.ReadAsStringAsync();
            }
            return geolocation;
        }

        private class GeoIP
        {
            [JsonProperty("ip")]
            public string ip { get; private set; }

            [JsonProperty("country_code")]
            public string country_code { get; private set; }

            [JsonProperty("country_name")]
            public string country_name { get; private set; }

            [JsonProperty("region_code")]
            public string region_code { get; private set; }

            [JsonProperty("region_name")]
            public string region_name { get; private set; }

            [JsonProperty("city")]
            public string city { get; private set; }

            [JsonProperty("zip_code")]
            public string zip_code { get; private set; }

            [JsonProperty("time_zone")]
            public string time_zone { get; private set; }

            [JsonProperty("latitude")]
            public string latitude { get; private set; }

            [JsonProperty("longitude")]
            public string longitude { get; private set; }

            [JsonProperty("metro_code")]
            public string metro_code { get; private set; }
        }

        private string BuildResponse()
        {
            bool MyZip = IsUsorCanadianZipCode2("49418");
            bool CanadaPostal = IsUsorCanadianZipCode2("K8N 5W6");
            bool SantaPostal = IsUsorCanadianZipCode2("H0H 0H0");
            bool WhiteHouseZip = IsUsorCanadianZipCode2("20500");
            bool AnotherPostal = IsUsorCanadianZipCode2("K8N5W6");
            bool Failed = IsUsorCanadianZipCode2("F8N 5I6");

            return "<html><body>" +
                "<table border=\"1\" cellpadding=\"5\" style=\"border-collapse:collapse;\">" +
                $"<tr><td>My Zip 49418</td><td>{MyZip}</td></tr>" +
                $"<tr><td>Canada K8N 5W6</td><td>{CanadaPostal}</td></tr>" +
                $"<tr><td>Santa H0H 0H0</td><td>{SantaPostal}</td></tr>" +
                $"<tr><td>White House 20500</td><td>{WhiteHouseZip}</td></tr>" +
                $"<tr><td>CA Postal w/o space K8N5W6</td><td>{WhiteHouseZip}</td></tr>" +
                $"<tr><td>Canada F8N 5I6</td><td>{Failed}</td></tr>" +
                "</table></body></html>";
        }

        string _usZipRegEx = @"^\d{5}(-\d{4})?$";
        string _caZipRegEx = @"^[ABCEGHJKLMNPRSTVXY]{1}\d{1}[A-Z]{1} *\d{1}[A-Z]{1}\d{1}$";

        private bool IsUsorCanadianZipCode2(string zipCode)
        {
            bool validZipCode = true;
            return validZipCode;
        }
    }
}
