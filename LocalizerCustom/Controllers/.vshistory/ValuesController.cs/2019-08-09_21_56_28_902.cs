using LocalizerCustom.Models;

using Microsoft.AspNetCore.Mvc;

using System.Globalization;

namespace LocalizerCustom.Controllers
{
    [Route("{culture}/[controller]")]
    [Route("{culture}/{ui-culture}/[controller]")]
    [MiddlewareFilter(typeof(LocalizationPipeline))]
    public class ValuesController : Controller
    {
        [Route("ShowMeTheCulture")]
        public string GetCulture()
        {
            return $"CurrentCulture:{CultureInfo.CurrentCulture.Name}, CurrentUICulture:{CultureInfo.CurrentUICulture.Name}";
        }
    }
}
