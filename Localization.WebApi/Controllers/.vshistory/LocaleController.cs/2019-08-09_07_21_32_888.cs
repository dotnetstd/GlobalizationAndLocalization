using AspNetCore.Localization.WebApi.Middlewares;

using Localization.WebApi.Infra;
using Localization.WebApi.Utils;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Localization.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocaleController : ControllerBase
    {
        private readonly IStringLocalizer _localizer;

        public LocaleController(IStringLocalizer<ShareResource> localizer)
        {
            this._localizer = localizer;
        }

        /// <summary>
        /// https://localhost:44367/api/Locale/Get/fa-IR
        /// </summary>
        /// <param name="locale"></param>
        /// <returns></returns>
        [Route("Get/{locale}")]
        [HttpGet]
        [MiddlewareFilter(typeof(LocalizationMiddleware), Order = 0)]
        public async Task<string> Get([FromRoute] string locale)
        {
            IEnumerable<LocalizedString> localizedStrs =
                this._localizer.GetAllStrings(includeParentCultures: true);
            return await localizedStrs.ToJsonStringAsync(isCamelLowerCaseForKey: true);
        }
    }
}
