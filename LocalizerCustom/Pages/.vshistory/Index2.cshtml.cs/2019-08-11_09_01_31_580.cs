using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using System.ComponentModel.DataAnnotations;

namespace LocalizerCustom.Pages
{
    public class Index2Model : PageModel
    {
        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Display(Name = "Test")]
            public string Test { get; set; }
        }

        public void OnGet()
        {

        }
    }
}