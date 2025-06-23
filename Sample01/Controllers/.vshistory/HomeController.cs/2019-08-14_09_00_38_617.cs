using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

using Sample01.Localization;
using Sample01.Models;

using System;
using System.Diagnostics;
using System.Globalization;

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

        public IActionResult SetCulture(string id = "en")
        {
            string culture = id;
            Response.Cookies.Append(
               CookieRequestCultureProvider.DefaultCookieName,
               CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
               new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
           );

            ViewData["Message"] = "Culture set to " + culture;

            return View("About");
        }

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

        public IActionResult Index()
        {
            PersianCalendar persianCalendar = new PersianCalendar();
            double charUnicodeInfo = CharUnicodeInfo.GetNumericValue('4');
            CompareInfo compareInfo = CompareInfo.GetCompareInfo("");
            CultureInfo cultureInfo = new CultureInfo("fa-IR");
            CultureNotFoundException cultureNotFoundException = new CultureNotFoundException();
            DateTimeFormatInfo dateTimeFormatInfo = new DateTimeFormatInfo();
            DaylightTime daylightTime = new DaylightTime(DateTime.Now, DateTime.Now.AddMinutes(30), TimeSpan.FromMinutes(4));
            EastAsianLunisolarCalendar eastAsianLunisolarCalendar = new ChineseLunisolarCalendar();
            //var globalizationExtensions = GlobalizationExtensions.GetStringComparer();
            IdnMapping idnMapping = new IdnMapping();
            NumberFormatInfo numberFormatInfo = new NumberFormatInfo();
            RegionInfo regionInfo = new RegionInfo("fa-IR");
            //SortKey sortKey = SortKey.Compare();
            //SortVersion sortVersion = new SortVersion();
            StringInfo stringInfo = new StringInfo();
            //TextElementEnumerator textElementEnumerator = new TextElementEnumerator();
            //TextInfo textInfo = TextInfo.ReadOnly();

            //CalendarAlgorithmType
            //CalendarWeekRule
            //CompareOptions
            //CultureAndRegionModifiers
            //CultureTypes
            //DateTimeStyles
            //DigitShapes
            //GregorianCalendarTypes
            //NumberStyles
            //TimeSpanStyles
            //UnicodeCategory

            string[] cultureNames = { "en-US", "en-GB", "fr-FR", "ne-NP", "es-BO", "ig-NG" };
            foreach (var cultureName in cultureNames)
            {
                RegionInfo region = new RegionInfo(cultureName);
                Console.WriteLine("{0} {1} the metric system.", region.EnglishName, region.IsMetric ? "uses" : "does not use");
            }







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
