using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

using ResourceReader.Models;

using System.Diagnostics;
using System.Reflection;
using System.Resources;
using System.Threading;

namespace ResourceReader.Controllers
{
    [Route("/Hello")]
    public class HelloController : Controller
    {
        private readonly IStringLocalizer<HelloController> _localizer;

        public HelloController(IStringLocalizer<HelloController> localizer)
        {
            _localizer = localizer;
        }

        // GET api/values
        [HttpGet]
        [Route("local")]
        public string local()
        {
            string msg = "";
            var cul = Thread.CurrentThread.CurrentCulture.Name;
            var culUI = Thread.CurrentThread.CurrentUICulture.Name;
            msg = _localizer["Hello"];
            var lang = Request.Headers["Accept-Language"];
            string rsFileName = "ResourceReader.Resources.rs1";
            ResourceManager rm = new ResourceManager(rsFileName, Assembly.GetExecutingAssembly());
            var msg2 = rm.GetString("Hello");
            var msg3 = rm.GetString("Hello", new System.Globalization.CultureInfo("fr-FR"));
            msg = msg2;
            return msg;
        }

        // GET api/values
        [HttpGet]
        [Route("lib")]
        public string lib()
        {
            string msg = "";
            msg = Messenger.GetHello();
            return msg;
        }
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

