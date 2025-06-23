using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Sample01.Localization
{
    public class RequestLocalizationMiddleware
    {
        public static IApplicationBuilder WithDynamicCultures(this IApplicationBuilder app)
        {
            var env = app.ApplicationServices.GetService<IHostingEnvironment>();
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("AppSettings.json");
            var config = builder.Build();
            var defaultCulture = config["Localization:DefaultCulture"];
            var cultures = config.GetSection("Localization:SupportedCultures")
                .AsEnumerable().Skip(1).ToList();
            var uiCultures = config.GetSection("Localization:SupportedUICultures")
                .AsEnumerable().Skip(1).ToList();
            var defaultRequestCulture = new RequestCulture(defaultCulture);
            var supportedCultures = new List<CultureInfo>();
            var supportedUICultures = new List<CultureInfo>();
            if (cultures.Count > 0)
            {
                supportedCultures = cultures.Select(i => new CultureInfo(i.Value)).ToList();
            }

            if (uiCultures.Count > 0)
            {
                supportedUICultures = uiCultures.Select(i => new CultureInfo(i.Value)).ToList();
            }

            var localizationOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>().Value;
            localizationOptions.DefaultRequestCulture = defaultRequestCulture;
            localizationOptions.SupportedCultures = supportedCultures;
            localizationOptions.SupportedUICultures = supportedUICultures;

            //return app.UseMiddleware<RequestLocalizationMiddleware>(Options.Create(localizationOptions));
            return app.UseMiddleware<RequestLocalizationMiddleware>(localizationOptions);
        }
    }
}
