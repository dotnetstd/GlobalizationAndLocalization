using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using System;
using System.Threading.Tasks;

namespace Sample01.Localization
{
    public class AppSettingsRequestCultureProvider : RequestCultureProvider
    {
        public string CultureKey { get; set; } = "AppOptions.Culture";

        public string UICultureKey { get; set; } = "AppOptions.UICulture";

        public override Task<ProviderCultureResult> DetermineProviderCultureResult(HttpContext httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException();
            }

            var configuration = httpContext.RequestServices.GetService<IConfigurationRoot>();
            var culture = configuration[CultureKey];
            var uiCulture = configuration[UICultureKey];

            if (culture == null && uiCulture == null)
            {
                return Task.FromResult((ProviderCultureResult)null);
            }

            if (culture != null && uiCulture == null)
            {
                uiCulture = culture;
            }

            if (culture == null && uiCulture != null)
            {
                culture = uiCulture;
            }

            var providerResultCulture = new ProviderCultureResult(culture, uiCulture);

            return Task.FromResult(providerResultCulture);
        }
    }
}
