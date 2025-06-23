using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;

using System.ComponentModel;
using System.Linq;

namespace LocalizerCustom.Models
{
    //public class LocalizedDataAnnotationsModelMetadataProvider : DataAnnotationsModelMetadataProvider
    //{
    //    protected override ModelMetadata CreateMetadata(IEnumerable<Attribute> attributes, Type containerType, Func<object> modelAccessor, Type modelType, string propertyName)
    //    {
    //        var meta = base.CreateMetadata(attributes, containerType, modelAccessor, modelType, propertyName);
    //        meta.DisplayName = Localizer[propertyName];
    //    }
    //}
    //public class LocalizedDisplayNameAttribute : DisplayNameAttribute
    //{
    //    public LocalizedDisplayNameAttribute(string resourceId)
    //        : base(GetMessageFromResource(resourceId))
    //    {

    //    }

    //    private static string GetMessageFromResource(string resourceId)
    //    {
    //        return LanguageManager.Read(resourceId);
    //    }
    //}

    //public class LoginViewModel
    //{
    //    [CustomRequiredAttribute("Email")]
    //    [LocalizedDisplayName("Email")]
    //    [EmailAddress]
    //    public string Email { get; set; }

    //    [LocalizedDisplayName("Password")]
    //    [CustomRequiredAttribute("Password")]
    //    [DataType(DataType.Password)]
    //    public string Password { get; set; }

    //    [Display(Name = "Remember me?")]
    //    public bool RememberMe { get; set; }

    //    [DisplayName("NameSurname")] public string name { get; set; }
    //}

    public class DisplayNameDetailsProvider : IDisplayMetadataProvider
    {
        public void CreateDisplayMetadata(DisplayMetadataProviderContext context)
        {
            var displayAttribute = context.Attributes.OfType<DisplayNameAttribute>().FirstOrDefault();
            if (displayAttribute != null)
            {
                context.DisplayMetadata.DisplayName = () => displayAttribute.DisplayName;
            }
        }
    }
}
