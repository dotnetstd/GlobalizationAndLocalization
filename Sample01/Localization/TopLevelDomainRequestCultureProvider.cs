using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;

using System;
using System.Linq;
using System.Threading.Tasks;

namespace Sample01.Localization
{
    public class TopLevelDomainRequestCultureProvider : RequestCultureProvider
    {
        public override Task<ProviderCultureResult> DetermineProviderCultureResult(HttpContext httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException(nameof(httpContext));
            }

            var hostFragments = httpContext.Request.Host.Host.Split('.');

            var culture = hostFragments.Last();

            var result = new ProviderCultureResult(culture);

            return Task.FromResult(result);
        }
    }
}
