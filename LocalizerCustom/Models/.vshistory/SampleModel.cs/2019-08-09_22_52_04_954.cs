using System.ComponentModel.DataAnnotations;

namespace LocalizerCustom.Models
{
    public class SampleModel
    {
        [Display(Name = "Hello")]
        public string Content { get; set; }
    }
}
