using LocalizerCustom.Models;

using Microsoft.AspNetCore.Mvc;

using System.Globalization;

namespace LocalizerCustom.Controllers
{
    [Route("{culture}/[controller]")]
    [Route("{culture}/{ui-culture}/[controller]")]
    [Route("{lang}/[controller]")]
    [Route("[controller]")]
    [MiddlewareFilter(typeof(LocalizationPipeline))]
    public class ValuesController : Controller
    {
        //overall route /{culture}/Values/ShowMeTheCulture
        [Route("ShowMeTheCulture")]
        public string GetCulture()
        {
            return $"CurrentCulture:{CultureInfo.CurrentCulture.Name}, CurrentUICulture:{CultureInfo.CurrentUICulture.Name}";
        }
    }
}
