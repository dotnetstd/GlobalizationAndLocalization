using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MainApp.Models.Localization
{
    public class JsonRequestCultureProvider : RequestCultureProvider
    {
        public static readonly string DefaultJsonFileName = "AppSettings.json";
        public static readonly string LocalizationSection = "Localization";
        public string JsonFileName { get; set; } = DefaultJsonFileName;
        public string CultureKey { get; set; } = "culture";
        public string UICultureKey { get; set; } = "ui-culture";
        public IConfigurationRoot Configuration { get; set; }
        public override Task<ProviderCultureResult> DetermineProviderCultureResult(HttpContext httpContext)
        {
            if (httpContext == null) throw new ArgumentNullException();
            var env = httpContext.RequestServices.GetService<IHostingEnvironment>();
            var builder = new ConfigurationBuilder().SetBasePath(env.ContentRootPath).AddJsonFile(JsonFileName);
            Configuration = builder.Build();
            string culture = null;
            string uiCulture = null;
            var localizationSection = Configuration.GetSection(LocalizationSection);
            if (!string.IsNullOrEmpty(CultureKey)) culture = localizationSection[CultureKey];
            if (!string.IsNullOrEmpty(UICultureKey)) uiCulture = localizationSection[UICultureKey];
            if (culture == null && uiCulture == null) return Task.FromResult((ProviderCultureResult)null);
            if (culture != null && uiCulture == null) uiCulture = culture;
            if (culture == null && uiCulture != null) culture = uiCulture;
            var providerResultCulture = new ProviderCultureResult(culture, uiCulture);
            return Task.FromResult(providerResultCulture);
        }
    }


    public class JsonRequestCultureProvider2 : RequestCultureProvider
    {
        public override Task<ProviderCultureResult> DetermineProviderCultureResult(HttpContext httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException(nameof(httpContext));
            }

            var config = Startup.LocalConfig;

            string culture = config["AppOptions:Culture"];
            string uiCulture = config["AppOptions:UICulture"];
            string culturedirection = config["AppOptions:CultureDirection"];

            culture = culture ?? "fa-IR"; // Use the value defined in config files or the default value
            uiCulture = uiCulture ?? culture;

            Startup.UiCulture = uiCulture;

            culturedirection = culturedirection ?? "rlt"; // rtl is set to be the default value in case culturedirection is null
            Startup.CultureDirection = culturedirection;

            return Task.FromResult(new ProviderCultureResult(culture, uiCulture));
        }
    }



}


