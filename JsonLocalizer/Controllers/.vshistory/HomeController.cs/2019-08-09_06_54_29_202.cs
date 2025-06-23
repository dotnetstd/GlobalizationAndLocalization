using JsonLocalizer.Models;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Localization;

using System;
using System.Diagnostics;

namespace JsonLocalizer.Controllers
{
	public class HomeController : Controller
	{
		private readonly IStringLocalizer _localizer;
		private readonly IHtmlLocalizer<HomeController> _localizer2;

		public HomeController(IStringLocalizerFactory localizer, IHtmlLocalizer<HomeController> localizer2)
		{
			_localizer = localizer.Create(null);
			_localizer2 = localizer2;
		}

		public IActionResult Index()
		{
			Response.Cookies.Append(
			   CookieRequestCultureProvider.DefaultCookieName,
			   CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(new System.Globalization.CultureInfo("ro-RO"))),
			   new CookieOptions
			   {
				   Expires = DateTimeOffset.Now.AddYears(1)
			   });

			string val = _localizer["Hello"];
			val = _localizer["Hello"];
			val = _localizer["Hello"];
			val = _localizer["Hello"];
			val = _localizer["Hello"];
			val = _localizer["Hello"];
			val = _localizer["Hello {0}", "Walera"];
			return View();
		}

		[HttpPost]
		public IActionResult SetLanguage(string culture, string returnUrl)
		{
			Response.Cookies.Append(
				CookieRequestCultureProvider.DefaultCookieName,
				CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
				new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
			);

			return LocalRedirect(returnUrl);
		}

		public IActionResult About()
		{
			ViewData["Message"] = "Your application description page.";

			return View();
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
