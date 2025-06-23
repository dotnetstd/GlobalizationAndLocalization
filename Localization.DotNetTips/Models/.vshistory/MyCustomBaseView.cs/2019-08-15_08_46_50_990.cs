using Microsoft.AspNetCore.Mvc.Razor;

using System.Globalization;

namespace Localization.DotNetTips.Models
{
    public abstract class MyCustomBaseView<TModel> : RazorPage<TModel>
    {
        protected MyCustomBaseView()
        {
            CultureInfo.CurrentCulture = new CultureInfo("fa-IR");
            CultureInfo.CurrentUICulture = new CultureInfo("fa-IR");
        }
    }
}
