using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace LocalizerCustom.Controllers
{
    public abstract class BaseController : Controller
    {
        private string _currentLanguage;

        public ActionResult RedirectToDefaultLanguage()
        {
            var lang = CurrentLanguage;
            if (lang == "et")
            {
                lang = "ee";
            }

            return RedirectToAction("Index", new { lang = lang });
        }

        private string CurrentLanguage
        {
            get
            {
                if (string.IsNullOrEmpty(_currentLanguage))
                {
                    var feature = HttpContext.Features.Get<IRequestCultureFeature>();
                    _currentLanguage = feature.RequestCulture.Culture.TwoLetterISOLanguageName.ToLower();
                }

                return _currentLanguage;
            }
        }
    }
}
