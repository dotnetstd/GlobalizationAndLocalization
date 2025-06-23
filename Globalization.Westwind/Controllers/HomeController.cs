using Globalization.Westwind.Models;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Diagnostics;

namespace Globalization.Westwind.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult ChangeLang(string data)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(data)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
                );

            var returnUrl = string.IsNullOrEmpty(Request.Path) ? "~/" : Request.Path.Value;
            return LocalRedirect("~/");
        }

        public IActionResult TestModelValidation()
        {
            var book = new Book();
            return View(book);
        }
    }
}
