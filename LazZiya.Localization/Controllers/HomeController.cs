using LazZiya.Localization.Models;

using Microsoft.AspNetCore.Mvc;

using System.Diagnostics;

namespace LazZiya.Localization.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index2()
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
