using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LocalizerCustom.Models
{
    public class SampleModel
    {
        [DisplayName("Hello")]
        public string Content { get; set; }
    }

    public class SampleModel2
    {
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Display(Name = "Name")]
        [StringLength(3, MinimumLength = 0, ErrorMessage = "Length of {0} field should be between {2} and {1}.")]
        public string Name { get; set; }
    }
}
