using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;

using System;
using System.Threading.Tasks;

namespace LocalizerCustom.Models
{
    public class ConfigurationRequestCultureProvider : RequestCultureProvider
    {
        public override Task<ProviderCultureResult>
        DetermineProviderCultureResult(HttpContext httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException(nameof(httpContext));
            }

            var builder = new ConfigurationBuilder();
            builder.AddJsonFile("appsettings.json");

            var config = builder.Build();
            string culture = config["culture"];
            string uiCulture = config["uiCulture"];

            culture = culture ?? "en-US";
            uiCulture = uiCulture ?? culture;

            return Task.FromResult(new ProviderCultureResult(culture, uiCulture));
        }
    }
}
