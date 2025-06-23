using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Aurochses.Xunit.AspNetCore.Mvc.Localization.Tests.Fakes.ModelLocalizationAssert.Validate.Models
{
    [ExcludeFromCodeCoverage]
    public class HomeViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email", Prompt = "Email.Prompt", Description = "Email.Description")]
        public string Email { get; set; }

        public string Token { get; set; }
    }
}