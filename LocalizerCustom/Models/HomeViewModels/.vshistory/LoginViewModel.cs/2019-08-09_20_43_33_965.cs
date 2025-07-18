using System.ComponentModel.DataAnnotations;

using static Microsoft.AspNetCore.Razor.Language.TagHelperMetadata;

namespace LocalizerCustom.Models.HomeViewModels
{
    public class LoginViewModel
    {
        [Display(Name = "Email", ResourceType = typeof(Common))]
        [Required(ErrorMessage = "EmailRequired")]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Password", ResourceType = typeof(Common))]
        [Required(ErrorMessage = "PasswordRequired")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
