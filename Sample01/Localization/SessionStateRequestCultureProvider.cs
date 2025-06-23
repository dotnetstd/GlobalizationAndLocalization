using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;

namespace MainApp.Models.Localization
{
    public class SessionStateRequestCultureProvider : RequestCultureProvider
    {
        public string SessionStateKey { get; set; } = "culture";
        public string UISessionStateKey { get; set; } = "ui-culture";
        public override Task<ProviderCultureResult> DetermineProviderCultureResult(HttpContext httpContext)
        {
            if (httpContext == null) throw new ArgumentNullException(nameof(httpContext));
            string culture = null;
            string uiCulture = null;
            if (!string.IsNullOrEmpty(SessionStateKey)) culture = httpContext.Session.GetString(SessionStateKey);
            if (!string.IsNullOrEmpty(UISessionStateKey)) uiCulture = httpContext.Session.GetString(UISessionStateKey);
            if (culture == null && uiCulture == null) return Task.FromResult((ProviderCultureResult)null);
            if (culture != null && uiCulture == null) uiCulture = culture;
            if (culture == null && uiCulture != null) culture = uiCulture;
            var providerResultCulture = new ProviderCultureResult(culture, uiCulture);
            return Task.FromResult(providerResultCulture);
        }
    }
}
