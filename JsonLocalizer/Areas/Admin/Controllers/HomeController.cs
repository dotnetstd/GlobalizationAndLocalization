using Microsoft.AspNetCore.Mvc;

namespace Mohmd.JsonResources.Example.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
