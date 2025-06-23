using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

using Sample01.Models;

using System.Diagnostics;

namespace Sample01.Controllers
{
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
			return culture.ToString();
		}


		public IActionResult Index()
		{
			ViewData["Welcome"] = _stringLocalizer["Kuy"].Value;
			ViewData["Welcome2"] = _stringLocalizer["Kuy"].Value;

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
