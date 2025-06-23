using MainApp.Models.Localization;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Localization;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

namespace Sample01.Controllers
{
    //[MiddlewareFilter(typeof(LocalizationPipeline))]
    public class ChandZabaneController : Controller
    {
        private readonly IStringLocalizer<ChandZabaneController> _localizerStr;

        private readonly IStringLocalizer<ChandZabaneController> _localizerString;
        private readonly IHtmlLocalizer<ChandZabaneController> _localizerHtml;

        private readonly IStringLocalizer _localizer;
        private readonly IStringLocalizer _localizer2;

        private readonly IStringLocalizer<SharedResource> _sharedLocalizerString;
        private readonly IHtmlLocalizer<SharedResource> _sharedLocalizerHtml;






        private readonly IStringLocalizer<ChandZabaneController> _stringLocalizer;
        private readonly IHtmlLocalizer<ChandZabaneController> _htmlLocalizer;
        private readonly IStringLocalizer<SharedResource> _stringSharedResources;
        private readonly IHtmlLocalizer<SharedResource> _htmlSharedResources;



        public ChandZabaneController(
                                IStringLocalizer<ChandZabaneController> stringLocalizer,
                                IStringLocalizer<ChandZabaneController> localizerString
                              , IHtmlLocalizer<ChandZabaneController> localizerHtml
                              , IStringLocalizer<SharedResource> sharedLocalizerString
                              , IHtmlLocalizer<SharedResource> sharedLocalizerHtml
                              , IStringLocalizerFactory factory,


                                 IHtmlLocalizer<ChandZabaneController> htmlLocalizer,
                                 IStringLocalizer<SharedResource> stringSharedResources,
                                 IHtmlLocalizer<SharedResource> htmlSharedResources

                             )
        {
            _localizerStr = stringLocalizer;
            var type = typeof(SharedResource);
            var assemblyName = new AssemblyName(type.GetTypeInfo().Assembly.FullName);
            _localizer = factory.Create(type);
            _localizer2 = factory.Create("SharedResource", assemblyName.Name);
            _localizerString = localizerString;
            _localizerHtml = localizerHtml;
            _sharedLocalizerString = sharedLocalizerString;
            _sharedLocalizerHtml = sharedLocalizerHtml;

            _stringLocalizer = stringLocalizer;
            _htmlLocalizer = htmlLocalizer;
            _stringSharedResources = stringSharedResources;
            _htmlSharedResources = htmlSharedResources;
        }

        public IActionResult Index(string culture = "fa-IR")
        {
            ViewData["Message"] = _localizer["MSBH"];
            return View();
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
            return View("Index");
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
        public IActionResult MSBH(string name)
        {
            //ViewBag.TsetSH = _sharedLocalizerString["SHRS"];
            //ViewBag.HCTRL = _localizerString["HCTRL"];
            return View();
        }

        [HttpPost]
        public IActionResult CreateMe(RegisterViewModel RVVM/*, string returnUrl*/)
        {
            RegisterViewModel RVME = new RegisterViewModel
            {
                Email = RVVM.Email,
                Password = RVVM.Password,
                ConfirmPassword = RVVM.ConfirmPassword
            };

            return View("MSBH", RVME);
            //return Redirect(returnUrl);
        }
        //public IActionResult RedirectToDefaultLanguage()
        //{
        //    var lang = CurrentLanguage;
        //    if (lang == "en") lang = "en";
        //    return RedirectToAction("Index", new { lang = lang });
        //}
        //private string CurrentLanguage {
        //    get {
        //        if (!string.IsNullOrEmpty(_currentLanguage))
        //        {
        //            return _currentLanguage;
        //        }
        //        if (RouteData.Values.ContainsKey("lang"))
        //        {
        //            _currentLanguage = RouteData.Values["lang"].ToString().ToLower();
        //            if (_currentLanguage == "ee")
        //            {
        //                _currentLanguage = "et";
        //            }
        //        }
        //        if (string.IsNullOrEmpty(_currentLanguage))
        //        {
        //            var feature = HttpContext.Features.Get<IRequestCultureFeature>();
        //            _currentLanguage = feature.RequestCulture.Culture.TwoLetterISOLanguageName.ToLower();
        //        }
        //        return _currentLanguage;
        //    }
        //}


        //////////////////////////////////

        #region snippet_MiddlewareFilter
        [Route("{culture}/[controller]/[action]")]
        //[MiddlewareFilter(typeof(LocalizationPipeline))]
        public IActionResult CultureFromRouteData()
        {
            return Content($"CurrentCulture:{CultureInfo.CurrentCulture.Name},"
                + $"CurrentUICulture:{CultureInfo.CurrentUICulture.Name}");
        }
        #endregion

        public IActionResult About()
        {
            IRequestCultureFeature requestCultureFeature = HttpContext.Features.Get<IRequestCultureFeature>();
            RequestCulture requestCulture = requestCultureFeature.RequestCulture;
            ViewData["requestCultureFeature"] = requestCultureFeature;
            ViewData["requestCulture"] = requestCulture;
            ViewData["Message"] = _localizer["Your application description page."];

            return View();
        }


        //////////////////////////////////


        public IActionResult SetCulture2(string id = "en")
        {
            string culture = id;
            Response.Cookies.Append(
               CookieRequestCultureProvider.DefaultCookieName,
               CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
               new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
           );

            ViewData["Message"] = "Culture set to " + culture;

            //return View("Index");
            return RedirectToAction("SinjulMSBH");
        }


        [HttpPost]
        public IActionResult SetLanguage2(string Culture, string ReturnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(Culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return LocalRedirect(ReturnUrl);
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SinjulMSBH()
        {
            //_logger.LogInformation(_stringLocalizer["Hello"]);
            //_logger.LogInformation(_stringSharedResources["SinjulMSBH"]);
            //?culture=fi-FI&ui-culture=fi-FI	?culture=fi-FI	?ui-culture=fi-FI

            //c=fi-FI|uic=fi-FI	c=fi-FI	uic=fi-FI

            //Accept-Language:en-US,en;q=0.8,fi;q=0.6

            string firstName = "Sinjul";
            string lastName = "MSBH";
            string localizedString = _stringLocalizer["Hello {0} {1}!", firstName, lastName];

            IStringLocalizer finnishLocalizer = _stringLocalizer.WithCulture(new CultureInfo("en-GB"));

            IEnumerable<LocalizedString> localizedStrings = _stringLocalizer.GetAllStrings(includeParentCultures: true);

            string sharedLocalizedString = _stringSharedResources["SinjulMSBH"];
            string HelloControoler = _stringLocalizer.GetString("Hello");
            ViewBag.HelloHtml = _htmlLocalizer[$"<b>{HelloControoler} {sharedLocalizedString}</b>"];
            ViewBag.HelloNormal = _stringLocalizer[$"<b>{HelloControoler} {sharedLocalizedString}</b>"];

            return View();
        }

        public IActionResult About2()
        {
            ViewData["Message"] = _localizer["SinjulMSBH"];

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult CreateContact()
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            return View();
        }

        public IActionResult MSBH2(string name)
        {
            //ViewBag.TsetSH = _sharedLocalizerString["SHRS"];
            //ViewBag.HCTRL = _localizerString["HCTRL"];

            return View();
        }

        [HttpPost]
        public IActionResult CreateMe2(RegisterViewModel RVVM/*, string returnUrl*/)
        {
            RegisterViewModel RVME = new RegisterViewModel
            {
                Email = RVVM.Email,
                Password = RVVM.Password,
                ConfirmPassword = RVVM.ConfirmPassword
            };

            return View("MSBH", RVME);
            //return Redirect(returnUrl);
        }
    }
}
