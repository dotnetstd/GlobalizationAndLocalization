using LazZiya.ExpressLocalization.Messages


namespace LazZiya.Localization.Models
{
    public class MyModel
    {
        [Required(ErrorMessage = DataAnnotationsErrorMessages.RequiredAttribute_ValidationError)]
        [StringLength(maximumLength: 25,
                    ErrorMessage = DataAnnotationsErrorMessages.StringLengthAttribute_ValidationErrorIncludingMinimum,
                    MinimumLength = 3)]
        [Display(Name = "Name")]
        public string Name { get; set; }
    }
}
