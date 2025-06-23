using AspNetCoreLocalization;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

using SqlLocalization.Filters;

namespace SqlLocalization.Controllers
{
    [ServiceFilter(typeof(LanguageActionFilter))]
    [Route("api/{culture}/[controller]")]
    public class AboutWithCultureInRouteController : Controller
    {
        // http://localhost:5000/api/it-CH/AboutWithCultureInRoute
        // http://localhost:5000/api/fr-CH/AboutWithCultureInRoute
        private readonly IStringLocalizer<SharedResource> _localizer;

        public AboutWithCultureInRouteController(IStringLocalizer<SharedResource> localizer)
        {
            _localizer = localizer;
        }

        [HttpGet]
        public string Get()
        {
            return _localizer["Name"];
        }
    }
}
