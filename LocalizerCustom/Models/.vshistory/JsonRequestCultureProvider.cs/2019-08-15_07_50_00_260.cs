using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;

using System;
using System.Threading.Tasks;

namespace Sample01.Models
{
    public class JsonRequestCultureProvider : RequestCultureProvider
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
