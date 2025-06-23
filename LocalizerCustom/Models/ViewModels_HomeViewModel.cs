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

    //public class homeviewmodel
    //{
    //    [required(errormessage = resourcekeys.required)]
    //    [emailaddress(errormessage = resourcekeys.notavalidemail)]
    //    [display(name = resourcekeys.youremail, resourcetype = typeof(resources.viewmodels_homeviewmodel))]
    //    public string email { get; set; }
    //}
    //public class homeviewmodel
    //{
    //    [required(errormessage = resourcekeys.required)]
    //    [emailaddress(errormessage = resourcekeys.notavalidemail)]
    //    [display(name = resourcekeys.youremail)]
    //    public string email { get; set; }
    //}
}
