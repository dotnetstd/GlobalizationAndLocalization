using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;

using System.Threading.Tasks;

namespace Localization.DotNetTips.Models
{
    public class FaRequestCultureProvider : RequestCultureProvider
    {
        public override Task<ProviderCultureResult> DetermineProviderCultureResult(HttpContext httpContext)
        {
            return Task.FromResult(new ProviderCultureResult("fa-IR"));
        }
    }
}
