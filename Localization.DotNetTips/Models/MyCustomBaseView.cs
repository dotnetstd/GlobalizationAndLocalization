using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Razor.Internal;

using System.Globalization;
using System.Threading.Tasks;

namespace Localization.DotNetTips.Models
{
    public abstract class MyCustomBaseView<TModel> : RazorPage<TModel>
    {
        protected MyCustomBaseView()
        {
            CultureInfo.CurrentCulture = new CultureInfo("fa-IR");
            CultureInfo.CurrentUICulture = new CultureInfo("fa-IR");
        }
    }
}

namespace TestResources.StartupCustomizations
{
    public abstract class MyCustomBaseView<TModel> : RazorPage<TModel>
    {
        //روش خاص تزریق وابستگی‌ها در فایل ویژه‌ی جاری
        [RazorInject]
        public IHtmlLocalizerFactory MyHtmlLocalizerFactory { get; set; }

        public IHtmlLocalizer MySharedLocalizer => MyHtmlLocalizerFactory.Create(
            baseName: "SharedResource" /*مشخصات*/,
            location: "TestResources.ExternalResources" /*نام اسمبلی ثالث*/);

        public bool IsAuthenticated()
        {
            return Context.User.Identity.IsAuthenticated;
        }

#pragma warning disable 1998
        public override async Task ExecuteAsync()
        {
        }
#pragma warning restore 1998
    }
}