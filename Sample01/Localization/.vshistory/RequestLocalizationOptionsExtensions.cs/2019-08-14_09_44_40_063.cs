using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;

using System;
using System.Collections.Generic;
using System.Globalization;

namespace Sample01.Localization
{
    public static class RequestLocalizationOptionsExtensions
    {
        public static RequestLocalizationOptions AddSupportedCultures(
            this RequestLocalizationOptions options,
            params string[] cultures)
        {
            var supportedCultures = new List<CultureInfo>();
            foreach (var culture in cultures)
            {
                supportedCultures.Add(new CultureInfo(culture));
            }
            options.SupportedCultures = supportedCultures;
            return options;
        }

        public static RequestLocalizationOptions AddSupportedUICultures(
            this RequestLocalizationOptions options,
            params string[] uiCultures)
        {
            var supportedUICultures = new List<CultureInfo>();
            foreach (var culture in uiCultures)
            {
                supportedUICultures.Add(new CultureInfo(culture));
            }
            options.SupportedUICultures = supportedUICultures;
            return options;
        }

        public static RequestLocalizationOptions SetDefaultCulture(
            this RequestLocalizationOptions options,
            string defaultCulture)
        {
            options.DefaultRequestCulture = new RequestCulture(defaultCulture);
            return options;
        }
    }
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseRequestLocalization(
            this IApplicationBuilder app,
            Action<RequestLocalizationOptions> optionsAction)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            if (optionsAction == null)
            {
                throw new ArgumentNullException(nameof(optionsAction));
            }
            var options = new RequestLocalizationOptions();
            optionsAction.Invoke(options);
            //return app.UseMiddleware<RequestLocalizationMiddleware>(Options.Create(options));
            return app.UseMiddleware<RequestLocalizationMiddleware>(options);
        }

        public static IApplicationBuilder UseRequestLocalization(
            this IApplicationBuilder app,
            params string[] cultures)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            if (cultures == null)
            {
                throw new ArgumentNullException(nameof(cultures));
            }

            if (cultures.Length == 0)
            {
                throw new ArgumentException(nameof(cultures));
            }
            var options = new RequestLocalizationOptions()
                .AddSupportedCultures(cultures)
                .AddSupportedUICultures(cultures)
                .SetDefaultCulture(cultures[0]);

            //return app.UseMiddleware<RequestLocalizationMiddleware>(Options.Create(options));
            return app.UseMiddleware<RequestLocalizationMiddleware>(options);
        }
    }
}
