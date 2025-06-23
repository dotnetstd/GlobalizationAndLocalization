using AspNetCore.Localization.WebApi.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using System.Globalization;

namespace AspNetCore.Localization.WebApi.Middlewares
{
    public class LocalizationMiddleware
    {
        private readonly string defaultCulture = "zh-TW";

        public static CultureInfo[] SupportedCultures = new[]{
            new CultureInfo("zh-TW"),
            new CultureInfo("zh-CN"),
            new CultureInfo("en-US"),
         };

        public void Configure(IApplicationBuilder app)
        {
            var options = new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(defaultCulture),
                // Formatting numbers, dates, etc.
                SupportedCultures = SupportedCultures,
                // UI strings that we have localized.
                SupportedUICultures = SupportedCultures
                //Clear default providers
            };
            options.RequestCultureProviders.Clear();
            options.RequestCultureProviders.Add(new RouteCultureProvider());

            app.UseRequestLocalization(options);
        }
    }
}
