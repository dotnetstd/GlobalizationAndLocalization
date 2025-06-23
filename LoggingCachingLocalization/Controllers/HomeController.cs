using LoggingCachingLocalization.Models;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using System;
using System.Diagnostics;

namespace LoggingCachingLocalization.Controllers
{
	public class HomeController : Controller
	{
		readonly ILogger<HomeController> _logger;

		public HomeController(ILogger<HomeController> logger)
		{
			_logger = logger;
		}

		public IActionResult Index()
		{
			_logger.LogInformation(1000, "################################ Index method invoked. ################################");

			DoSomething();
			return View();
		}

		// Logging scope sample
		void DoSomething()
		{
			using (_logger.BeginScope("################################ DoSomething scope! ################################"))
			{
				_logger.LogInformation(1000, "################################ DoSomething method invoked. ################################");

				_logger.LogWarning(1002, "################################ Something happened! ################################");
			}
		}

		//Localization sample
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
