using LocalizationCore;

using Microsoft.AspNetCore.Mvc;

namespace JsonLocalizer.Controllers
{
    public class TestController : CultureMatchingController
    {
        public IActionResult Index()
        {
            return View();
        }

        public string TE()
        {
            return "STRING CONTENT";
        }
    }
}