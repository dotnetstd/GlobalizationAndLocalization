using System.Globalization;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Microsoft.VisualBasic;

namespace MainApp.Models.Localization
{
    public class CustomLocalizer : StringLocalizer<Strings>
    {
        private readonly IStringLocalizer _internalLocalizer; public CustomLocalizer(IStringLocalizerFactory factory, IHttpContextAccessor httpContextAccessor) : base(factory)
        {
            CurrentLanguage = httpContextAccessor.HttpContext.GetRouteValue("lang") as string;
            if (string.IsNullOrEmpty(CurrentLanguage) || CurrentLanguage == "en") CurrentLanguage = "en";
            _internalLocalizer = WithCulture(new CultureInfo(CurrentLanguage));
        }
        public override LocalizedString this[string name, params object[] arguments] => _internalLocalizer[name, arguments];
        public override LocalizedString this[string name] => _internalLocalizer[name];
        public string CurrentLanguage { get; set; }
    }

}
