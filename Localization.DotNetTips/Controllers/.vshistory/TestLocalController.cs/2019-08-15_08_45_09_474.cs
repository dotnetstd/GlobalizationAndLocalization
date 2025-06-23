using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Localization;

using System;
using System.Globalization;

namespace TestResources.Controllers
{
    public class TestLocalController : Controller
    {
        //private readonly IStringLocalizer<TestLocalController> _stringLocalizer;
        //private readonly IHtmlLocalizer<TestLocalController> _htmlLocalizer;

        private readonly IStringLocalizer _stringLocalizer;
        private readonly IHtmlLocalizer _htmlLocalizer;

        public TestLocalController(
            //IStringLocalizer<TestLocalController> stringLocalizer,
            //IHtmlLocalizer<TestLocalController> htmlLocalizer
            IStringLocalizerFactory stringLocalizerFactory,
            IHtmlLocalizerFactory htmlLocalizerFactory)
        {
            _stringLocalizer = stringLocalizerFactory.Create(
                baseName: "Controllers.TestLocalController" /*مشخصات كنترلر جاري*/,
                location: "TestResources.ExternalResources" /*نام اسمبلي ثالث*/);

            _htmlLocalizer = htmlLocalizerFactory.Create(
                baseName: "Controllers.TestLocalController" /*مشخصات كنترلر جاري*/,
                location: "TestResources.ExternalResources" /*نام اسمبلي ثالث*/);
        }

        public IActionResult Index()
        {
            var name = "DNT";
            var message = _htmlLocalizer["<b>Hello</b><i> {0}</i>", name];
            ViewData["Message"] = message;
            return View();
        }

        [HttpGet]
        public string GetTitle()
        {
            var about = _stringLocalizer["About Title"];
            return about;
        }

        public IActionResult SetFaLanguage()
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(new CultureInfo("fa-IR"))),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return RedirectToAction("GetTitle");
        }
    }
}