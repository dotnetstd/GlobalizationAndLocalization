using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

using Sample01.Localization;
using Sample01.Models;

using System;
using System.Diagnostics;

namespace Sample01.Controllers
{
	[CultureFilter]
	public class HomeController : Controller
	{
		private readonly IStringLocalizer<HomeController> _stringLocalizer;
		public HomeController(IStringLocalizer<HomeController> stringLocalizer)
		{
			_stringLocalizer = stringLocalizer;
		}

		public string Chen()
		{
			return _stringLocalizer.GetString("Hello");
		}

		public string Chen2()
		{
			var culture = Request.HttpContext.Features.Get<IRequestCultureFeature>().RequestCulture.Culture;
			var requestCultureFeature = HttpContext.Features.Get<IRequestCultureFeature>();
			var culture2 = requestCultureFeature.RequestCulture.UICulture;
			return culture.ToString();
		}

		public IActionResult SetThai()
		{
			var cookieValue = CookieRequestCultureProvider.MakeCookieValue(new RequestCulture("th-TH"));
			var option = new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) };

			Response.Cookies.Append("Web.Language", cookieValue, option);
			return RedirectToAction(nameof(Index));
		}

		public IActionResult SetEnglish()
		{
			var cookieValue = CookieRequestCultureProvider.MakeCookieValue(new RequestCulture("en-GB"));
			var option = new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) };

			Response.Cookies.Append("Web.Language", cookieValue, option);
			return RedirectToAction(nameof(Index));
		}

		public IActionResult Index()
		{
			ViewData["Welcome"] = _stringLocalizer["Kuy"].Value;
			ViewData["Welcome2"] = _stringLocalizer["Kuy"].Value;

			var model = new TestModel { TestString1 = CoreLocalizationTest.Resources.Resource.Test };

			return View(model);
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
