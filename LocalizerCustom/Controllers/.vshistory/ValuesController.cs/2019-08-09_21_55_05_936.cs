using Microsoft.AspNetCore.Mvc;

using System.Globalization;

namespace LocalizerCustom.Controllers
{
    [Route("{culture}/[controller]")]
    public class ValuesController : Controller
    {
        [Route("ShowMeTheCulture")]
        public string GetCulture()
        {
            return $"CurrentCulture:{CultureInfo.CurrentCulture.Name}, CurrentUICulture:{CultureInfo.CurrentUICulture.Name}";
        }
    }
}
