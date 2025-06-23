using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Mvc.RazorPages;

using System.ComponentModel.DataAnnotations;

namespace LocalizerCustom.Pages
{
    public class Index2Model : PageModel
    {
        private readonly IHtmlLocalizer _loc;

        public Index2Model(IHtmlLocalizerFactory htmlLocalizerFactory)
        {
            _loc = htmlLocalizerFactory.Create("LocalizerCustom.Pages.Index2", "RazorPagesLocalization");
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Display(Name = "Test")]
            public string Test { get; set; }
        }

        public IHtmlContent Message { get; set; }

        public void OnGet()
        {
            Message = _loc["Hello"];
        }
    }
}