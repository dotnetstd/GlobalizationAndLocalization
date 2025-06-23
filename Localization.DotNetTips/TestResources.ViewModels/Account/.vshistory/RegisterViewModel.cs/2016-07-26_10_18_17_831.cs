using System.ComponentModel.DataAnnotations;
using Core1RtmTestResources.ExternalResources.Resources;

namespace Core1RtmTestResources.ViewModels.Account
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "EmailReq")]
        [EmailAddress(ErrorMessage = "EmailType")]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}