using Localization.DotNetTips.Models;

using Microsoft.AspNetCore.Mvc;

using System.Diagnostics;

namespace Localization.DotNetTips.Controllers
{
    public class HomeController : Controller
    {
        [FaLanguageActionFilter]
        public IActionResult Index()
        {
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
