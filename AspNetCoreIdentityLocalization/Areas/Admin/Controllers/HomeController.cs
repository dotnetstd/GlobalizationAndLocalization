using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace AspNetCoreIdentityLocalization.Areas.Admin.Controllers
{
	[Area("admin")]
	public class HomeController : Controller
	{
		private readonly IStringLocalizer<HomeController> _stringLocalizer;

		public HomeController(IStringLocalizer<HomeController> stringLocalizer)
		{
			_stringLocalizer = stringLocalizer;
		}

		public IActionResult Index()
		{
			ViewBag.Message = _stringLocalizer["admin-message"];
			return View();
		}
	}
}