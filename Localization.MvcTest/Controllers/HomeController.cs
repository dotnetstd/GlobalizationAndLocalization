using Localization.MvcTest.Infrastructure;
using Localization.MvcTest.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

using Raveshmand.Localization.Core;

using System.Diagnostics;
using System.Globalization;

namespace Localization.MvcTest.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILocalizerCrud _localizerCrud;
        public HomeController(ILocalizerCrud localizerCrud, IStringLocalizerFactory localizerFactory) : base(localizerFactory)
        {
            _localizerCrud = localizerCrud;
        }

        public IActionResult Index()
        {
            string value = L("Test01");

            _localizerCrud.Insert("Test01", "Test Value", CultureInfo.CurrentCulture.Name, LocalizationResourceName);

            value = L("Test01");

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
