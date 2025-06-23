using ASPNETCoreAndESearch.Business;
using ASPNETCoreAndESearch.Models;

using LocalizerCustom.Models;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Threading.Tasks;

namespace LocalizerCustom.Controllers
{
    [MiddlewareFilter(typeof(LocalizationPipeline))]
    [MiddlewareFilter(typeof(CultureMiddleware))]
    public class HomeController : Controller
    {
        private readonly IProductService _productService;


        private readonly IStringLocalizer<HomeController> _localizer2;
        private string _currentLanguage;
        private string CurrentLanguage
        {
            get
            {
                if (!string.IsNullOrEmpty(_currentLanguage))
                    return _currentLanguage;

                if (string.IsNullOrEmpty(_currentLanguage))
                {
                    var feature = HttpContext.Features.Get<IRequestCultureFeature>();
                    _currentLanguage = feature.RequestCulture.Culture.TwoLetterISOLanguageName.ToLower();
                }

                return _currentLanguage;
            }
        }
        public ActionResult RedirectToDefaultCulture()
        {
            var culture = CurrentLanguage;
            if (culture != "en")
                culture = "en";

            return RedirectToAction("Index", new { culture });
        }

        private readonly IStringLocalizer _localizer;
        private readonly IStringLocalizer _sharedLocalizer;

        public HomeController(
            IStringLocalizer<HomeController> localizer,
            IStringLocalizer<SharedResource> sharedLocalizer,
            IProductService productService)
        {
            _localizer = localizer;
            _localizer2 = localizer;
            _sharedLocalizer = sharedLocalizer;
            _productService = productService;
        }

        public IActionResult Index()
        {
            // string indexName = $"products";
            // await FeedProductsInTurkish(indexName, "tr");
            // await FeedProductsInEnglish(indexName, "en");

            return View(model: new SampleModel());
        }

        public IActionResult Index2(string culture)
        {
            var ci = new CultureInfo(culture ?? "en-US");

            CultureInfo.CurrentCulture = ci;
            CultureInfo.CurrentUICulture = ci;

            return View();
        }


        public IActionResult Router(string culture, string slug1, string slug2)
        {
            // Get content by latest slug
            if (!string.IsNullOrEmpty(slug2)) { return ReturnSlugAction(slug2, culture); }

            if (!string.IsNullOrEmpty(slug1)) { return ReturnSlugAction(slug1, culture); }

            return RedirectPermanent("/");
        }

        // Generate slug base class depends on your slug strategy
        public class Translation
        {
            public int Id { get; set; }

            public string Slug { get; set; }

            public string Culture { get; set; }

            public string Description { get; set; }

            // depends on your great strategy, you can add props...
            public string MetaTitle { get; set; }

            public string MetaDescription { get; set; }

            public string MetaKeywords { get; set; }

            // Even you may retrieve related controller and action from slug base class
        }

        //private IActionResult ReturnSlugAction(string slug, string culture)
        //{
        //    culture = string.IsNullOrEmpty(culture) ? "en" : culture;
        //    var data = _appDbContext.Data.FirstOrDefault(x => x.Translations.Exists(tr => tr.Slug == slug && tr.Culture == culture));

        //    // if slug is not exist return home page
        //    if (data == null) { return RedirectPermanent("/"); }

        //    data.ViewTranslations = data.Translations.FirstOrDefault(x => x.Culture == culture);
        //    var typeName = data.GetType().Name;
        //    var controllerName = typeName + "Controller";
        //    var type = Type.GetType("slug.based.routing.Controllers." + controllerName);
        //    var instance = Activator.CreateInstance(type);

        //    var methodInfo = type.GetMethod("Index");
        //    var view = "~/Views/" + typeName + "/Index.cshtml";

        //    return methodInfo.Invoke(instance, new object[] { view, data }) as IActionResult;
        //}



        public string Content()
        {
            return $"CurrentCulture: {CultureInfo.CurrentCulture.Name}\r\n"
                 + $"CurrentUICulture: {CultureInfo.CurrentUICulture.Name}\r\n"
                 + $"Resources\\Controllers\\HomeController: {_localizer["Hello"]}\r\n"
                 + $"Resources\\SharedResource: {_sharedLocalizer["Hello"]}";
        }

        public IActionResult SetCulture(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return LocalRedirect(returnUrl);
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contacts()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [Route("/")]
        [MiddlewareFilter(typeof(LocalizationPipeline))]
        public IActionResult Index3() => View();


        public async Task<IActionResult> Products(string keyword)
        {
            var requestCultureFeature = Request.HttpContext.Features.Get<IRequestCultureFeature>();
            CultureInfo culture = requestCultureFeature.RequestCulture.Culture;

            ProductSearchResponse productSearchResponse = await _productService.SearchAsync(keyword, culture.TwoLetterISOLanguageName);

            return View(productSearchResponse);
        }

        private async Task FeedProductsInTurkish(string indexName, string lang)
        {
            List<Product> products = new List<Product>
            {
                new Product
                {
                    ProductId = 1,
                    Name = "Iphone X Cep Telefonu",
                    Description = "Gümüş renk, 128 GB",
                    Price = 6000
                },
                new Product
                {
                    ProductId = 2,
                    Name = "Iphone X Cep Telefonu",
                    Description = "Uzay grisi rengi, 128 GB",
                    Price = 6000
                },
                new Product
                {
                    ProductId = 3,
                    Name = "Rayban Erkek Gözlük",
                    Description = "Yeşil renk"
                },
                new Product
                {
                    ProductId = 4,
                    Name = "Rayban Kadın Gözlük",
                    Description = "Gri renk"
                }
            };

            await _productService.CreateIndexAsync($"{indexName}_{lang}");
            await _productService.IndexAsync(products, lang);
        }

        private async Task FeedProductsInEnglish(string indexName, string lang)
        {
            List<Product> products = new List<Product>
            {
                new Product
                {
                    ProductId = 1,
                    Name = "Iphone X Mobile Phone",
                    Description = "Silver color, 128 GB",
                    Price = 6000
                },
                new Product
                {
                    ProductId = 2,
                    Name = "Iphone X Mobile Phone",
                    Description = "Space gray color, 128 GB",
                    Price = 6000
                },
                new Product
                {
                    ProductId = 3,
                    Name = "Rayban Men's Glasses",
                    Description = "Green color"
                },
                new Product
                {
                    ProductId = 4,
                    Name = "Rayban Women's Glasses",
                    Description = "Gray color"
                }
            };

            await _productService.CreateIndexAsync($"{indexName}_{lang}");
            await _productService.IndexAsync(products, lang);
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
