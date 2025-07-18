using System.ComponentModel.DataAnnotations;

namespace LocalizationRouting.Models
{
    public class ContactFormViewModel
    {
        [Display(Name = "Ad Soyad")]
        [Required(ErrorMessage = "{0} gereklidir.")]
        public string AdSoyad { get; set; }

        [Display(Name = "Mesaj")]
        [Required(ErrorMessage = "{0} gereklidir.")]
        public string Mesaj { get; set; }
    }
}
