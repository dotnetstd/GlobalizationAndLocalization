using LocalizationDemo;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Resources;

namespace LocalizerCustom.Controllers
{
    //
    // This doesn't work.
    //
    //[ServiceFilter(typeof(LanguageActionFilter))]
    //[Route("{culture}/[controller]")]
    public class HomeWithCultureInRouteController : Controller
    {
        private readonly IStringLocalizer<HomeController> _localizer;
        private readonly IHtmlLocalizer<HomeController> _htmlLocalizer;
        private readonly IHtmlLocalizer<SharedResource> _sharedHtmlLocalizer;

        public HomeWithCultureInRouteController(IStringLocalizer<HomeController> localizer, IHtmlLocalizer<HomeController> htmlLocalizer, IHtmlLocalizer<SharedResource> sharedHtmlLocalizer)
        {
            _localizer = localizer;
            _htmlLocalizer = htmlLocalizer;
            _sharedHtmlLocalizer = sharedHtmlLocalizer;
        }

        [HttpGet]
        public IActionResult Index()
        {
            Student student = new Student();
            ViewData["StringLocalizer"] = _localizer["String localizer"];
            ViewData["StringLocalizerWithParameter"] = _localizer["String localizer with parameter - {0}", "Parameter"];
            ViewData["HtmlLocalizer"] = _htmlLocalizer["<b>Test html localizer</b>"];
            ViewData["HtmlLocalizerWithParameter"] = _htmlLocalizer["<b>Test html localizer with parameter</b> <i>{0}</i>", "Parameter"];
            ViewData["SharedHtmlLocalizer"] = _sharedHtmlLocalizer["<b>Shared html localizer</b>"];
            ViewData["SharedHtmlLocalizerWithParameter"] = _sharedHtmlLocalizer["<b>Shared html localizer with paramter</b> <i>{0}</i>", "Parameter"];
            return View(student);
        }

        [HttpPost]
        public IActionResult Index(Student student)
        {
            ViewData["StringLocalizer"] = _localizer["String localizer"];
            ViewData["StringLocalizerWithParameter"] = _localizer["String localizer with parameter - {0}", "Parameter"];
            ViewData["HtmlLocalizer"] = _htmlLocalizer["<b>Test html localizer</b>"];
            ViewData["HtmlLocalizerWithParameter"] = _htmlLocalizer["<b>Test html localizer with parameter</b> <i>{0}</i>", "Parameter"];
            ViewData["SharedHtmlLocalizer"] = _sharedHtmlLocalizer["<b>Shared html localizer</b>"];
            ViewData["SharedHtmlLocalizerWithParameter"] = _sharedHtmlLocalizer["<b>Shared html localizer with paramter</b> <i>{0}</i>", "Parameter"];
            return View(student);
        }
    }
}




namespace LocalizationDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly IStringLocalizer<HomeController> _localizer;
        private readonly IHtmlLocalizer<HomeController> _htmlLocalizer;
        private readonly IHtmlLocalizer<SharedResource> _sharedHtmlLocalizer;

        public HomeController(IStringLocalizer<HomeController> localizer, IHtmlLocalizer<HomeController> htmlLocalizer, IHtmlLocalizer<SharedResource> sharedHtmlLocalizer)
        {
            _localizer = localizer;
            _htmlLocalizer = htmlLocalizer;
            _sharedHtmlLocalizer = sharedHtmlLocalizer;
        }

        [HttpGet]
        public IActionResult Index()
        {
            Student student = new Student();
            ViewData["StringLocalizer"] = _localizer["String localizer"];
            ViewData["StringLocalizerWithParameter"] = _localizer["String localizer with parameter - {0}", "Parameter"];
            ViewData["HtmlLocalizer"] = _htmlLocalizer["<b>Test html localizer</b>"];
            ViewData["HtmlLocalizerWithParameter"] = _htmlLocalizer["<b>Test html localizer with parameter</b> <i>{0}</i>", "Parameter"];
            ViewData["SharedHtmlLocalizer"] = _sharedHtmlLocalizer["<b>Shared html localizer</b>"];
            ViewData["SharedHtmlLocalizerWithParameter"] = _sharedHtmlLocalizer["<b>Shared html localizer with paramter</b> <i>{0}</i>", "Parameter"];

            student.SelectedCulture = HttpContext.Request.Cookies[CookieRequestCultureProvider.DefaultCookieName];
            return View(student);
        }

        [HttpPost]
        public IActionResult Index(Student student)
        {
            ViewData["StringLocalizer"] = _localizer["String localizer"];
            ViewData["StringLocalizerWithParameter"] = _localizer["String localizer with parameter - {0}", "Parameter"];
            ViewData["HtmlLocalizer"] = _htmlLocalizer["<b>Test html localizer</b>"];
            ViewData["HtmlLocalizerWithParameter"] = _htmlLocalizer["<b>Test html localizer with parameter</b> <i>{0}</i>", "Parameter"];
            ViewData["SharedHtmlLocalizer"] = _sharedHtmlLocalizer["<b>Shared html localizer</b>"];
            ViewData["SharedHtmlLocalizerWithParameter"] = _sharedHtmlLocalizer["<b>Shared html localizer with paramter</b> <i>{0}</i>", "Parameter"];
            student.SelectedCulture = HttpContext.Request.Cookies[CookieRequestCultureProvider.DefaultCookieName];
            return View(student);
        }

        [HttpPost]
        public IActionResult SetCulture(string SelectedCulture)
        {
            //RequestCulture requestCulture = new RequestCulture(culture);
            //string cookieCulture = CookieRequestCultureProvider.MakeCookieValue(requestCulture);
            CookieOptions cookieOptions = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(1)
            };
            Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName, SelectedCulture);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult TestViewLocalizer()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}


namespace LocalizationDemo.Models
{
    public static class EnumHelper
    {
        public static string DisplayName(this Enum enumValue)
        {
            var enumType = enumValue.GetType();
            var memberInfo = enumType.GetMember(enumValue.ToString()).First();

            if (memberInfo == null || !memberInfo.CustomAttributes.Any()) return enumValue.ToString();

            var displayAttribute = memberInfo.GetCustomAttribute<DisplayAttribute>();

            if (displayAttribute == null) return enumValue.ToString();

            if (displayAttribute.ResourceType != null && displayAttribute.Name != null)
            {
                var manager = new ResourceManager(displayAttribute.ResourceType);
                return manager.GetString(displayAttribute.Name);
            }

            return displayAttribute.Name ?? enumValue.ToString();
        }
    }
}



namespace LocalizationDemo.Models
{
    public enum GenderType
    {
        [Display(Name = "None", ResourceType = typeof(Resources.DataAnnotations))]
        None,
        [Display(Name = "Male", ResourceType = typeof(Resources.DataAnnotations))]
        Male,
        [Display(Name = "Female", ResourceType = typeof(Resources.DataAnnotations))]
        Female
    }
    public class Student
    {
        [Required(ErrorMessage = "Name is required")]
        [MinLength(5, ErrorMessage = "Minimul length is 5")]
        [Display(Name = "Name", ResourceType = typeof(Resources.DataAnnotations))]
        public string Name { get; set; }

        [Required(ErrorMessage = "Surname is required")]
        [MinLength(5, ErrorMessage = "Minimul length is 5")]
        [Display(Name = "Surname", ResourceType = typeof(Resources.DataAnnotations))]
        public string Surname { get; set; }

        [Display(Name = "Gender", ResourceType = typeof(Resources.DataAnnotations))]
        public GenderType Gender { get; set; }

        public string SelectedCulture { get; set; }
    }
}



namespace LocalizationDemo.Models
{
    public class DropDownLists
    {
        public static SelectList GenderList
        {
            get
            {
                //
                // Don't cache because we don't get the translated element
                // when culture changes.
                //
                //if (_GemderSelectList != null)
                //{
                //    return _GemderSelectList;
                //}

                List<SelectListItem> listItems = new List<SelectListItem>();

                SelectListItem empty = new SelectListItem();
                empty.Value = "";
                empty.Text = "";
                listItems.Add(empty);

                SelectListItem male = new SelectListItem();
                male.Text = GenderType.Male.DisplayName();
                male.Value = GenderType.Male.ToString();
                listItems.Add(male);

                SelectListItem female = new SelectListItem();
                female.Text = GenderType.Female.DisplayName();
                female.Value = GenderType.Female.ToString();
                listItems.Add(female);

                _GemderSelectList = new SelectList(listItems, "Value", "Text");
                return _GemderSelectList;
            }
        }
        private static SelectList _GemderSelectList;
    }
}
