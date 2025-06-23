using Microsoft.AspNetCore.Mvc.Filters;

using System.Globalization;

namespace Localization.DotNetTips.Models
{
    public class FaLanguageActionFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            CultureInfo.CurrentCulture = new CultureInfo("fa-IR");
            CultureInfo.CurrentUICulture = new CultureInfo("fa-IR");
            base.OnActionExecuting(context);
        }
    }
}
