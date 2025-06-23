using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace AspNetCore.Localization.WebApi.Utils
{
    public class RouteCultureProvider : RequestCultureProvider
    {
        private const string defaultCulture = "zh-TW";
        public override Task<ProviderCultureResult> DetermineProviderCultureResult(HttpContext httpContext)
        {
            object finalCulture = string.Empty;
            if (httpContext == null)
            {
                throw new ArgumentNullException(nameof(httpContext));
            }


            //if (!httpContext.User.Identity.IsAuthenticated)
            //{
            //    return Task.FromResult((ProviderCultureResult)null);
            //}

            try
            {
                using (var routeMatcher = new RouteMatcher())
                {
                    PathString path = httpContext.Request.Path;
                    var template = "api/Locale/Get/{locale}";
                    var routeValues = routeMatcher.Matches(template, path.Value);
                    routeValues.TryGetValue("locale", out finalCulture);
                }
            }
            catch (Exception)
            {
                finalCulture = defaultCulture;
            }
            finally
            {
                finalCulture = finalCulture ?? defaultCulture;
            }

            return Task.FromResult(new ProviderCultureResult(finalCulture as string));

        }
    }
}