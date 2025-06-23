namespace LocalizerCustom.Models
{
    public class ViewModels_HomeViewModel
    {

        private static global::System.Resources.ResourceManager resourceMan;
        private static global::System.Globalization.CultureInfo resourceCulture;

        // details hidden for brevity

        public static string NotAValidEmail
        {
            get
            {
                return resourceMan.GetString("NotAValidEmail", resourceCulture);
            }
        }

        public static string Required
        {
            get
            {
                return resourceMan.GetString("Required", resourceCulture);
            }
        }

        public static string YourEmail
        {
            get
            {
                return resourceMan.GetString("YourEmail", resourceCulture);
            }
        }
    }

    //public class HomeViewModel
    //{
    //    [Required(ErrorMessage = ResourceKeys.Required)]
    //    [EmailAddress(ErrorMessage = ResourceKeys.NotAValidEmail)]
    //    [Display(Name = ResourceKeys.YourEmail, ResourceType = typeof(Resources.ViewModels_HomeViewModel))]
    //    public string Email { get; set; }
    //}
    //public class HomeViewModel
    //{
    //    [Required(ErrorMessage = ResourceKeys.Required)]
    //    [EmailAddress(ErrorMessage = ResourceKeys.NotAValidEmail)]
    //    [Display(Name = ResourceKeys.YourEmail)]
    //    public string Email { get; set; }
    //}
}
