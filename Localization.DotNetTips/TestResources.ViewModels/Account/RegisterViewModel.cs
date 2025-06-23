using System.ComponentModel.DataAnnotations;

namespace TestResources.ViewModels.Account
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "EmailReq")]
        [EmailAddress(ErrorMessage = "EmailType")]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}