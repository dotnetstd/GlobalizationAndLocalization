using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using System;
using System.ComponentModel.DataAnnotations;

namespace AppDemoConventionalMetadataProviders.Pages
{
	public class IndexModel : PageModel
	{
		[BindProperty]
		public InputModel Input { get; set; }

		public class InputModel
		{

			[Required, MaxLength(50)]
			public string Name { get; set; }

			[Required, EmailAddress]
			public string Email { get; set; }

			[Phone]
			public string Phone { get; set; }

			[Required, Range(1, 100)]
			public int Priority { get; set; }

			[Required, DataType(DataType.Date)]
			public DateTime BirthDate { get; set; }

			[Url]
			public string WebSite { get; set; }

			[RegularExpression(@"^\d{4}-[A-Z]{4}$", ErrorMessageResourceName = "OrderNumber_RegularExpression", ErrorMessageResourceType = typeof(AppDemoConventionalMetadataProviders.Resources.AdditionalValidation)), MaxLength(9)]
			public string OrderNumber { get; set; }

			public float Weight { get; set; }

		}

		public ActionResult OnPost()
		{
			var isValid = this.ModelState.IsValid;
			return this.Page();
		}
	}
}
