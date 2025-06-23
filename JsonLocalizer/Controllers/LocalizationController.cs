using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

using System;

namespace JsonLocalizer.Controllers
{
    public class LocalizationController : Controller
    {

        // string culture is set to a culture value (ex: en-GB or sv-SE) //
        // string returnUrl is the url the current route values (ex: localhost:5000/home/aboout) or "home/contact" //

        [HttpPost]
        public IActionResult SetCultureCookie(string culture, string returnUrl)
        {
            // Add a localization cookie to the response that will contain the culture information //
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );
            // Redirect to the page that this method was called from by parameter 'returnUrl' //
            return LocalRedirect(returnUrl);
        }
    }
}