using System;
using System.ComponentModel.DataAnnotations;

namespace MainApp.Models.Localization
{
    public class PersonViewModel
    {
        [Display(Name = "Name")]
        [Required(ErrorMessage = "{0} is required")]
        public string Name { get; set; }

        [Display(Name = "Age")]
        [Range(1, 99, ErrorMessage = "{0} must be a number between {1} and {2}")]
        [Required(ErrorMessage = "{0} is required")]
        public int Age { get; set; }
    }
}