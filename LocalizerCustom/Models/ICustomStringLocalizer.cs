using Microsoft.Extensions.Localization;

using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

namespace LocalizerCustom.Models
{
    public interface ICustomStringLocalizer : IStringLocalizer
    {
        LocalizedString GetLocalizedHtmlString(string key);
    }
    public class LocalizatonTranslationService : ICustomStringLocalizer
    {
        private readonly IStringLocalizer _localizer;

        public LocalizatonTranslationService(IStringLocalizerFactory factory)
        {
            var type = typeof(Startup); //EntityValidationResource
            var assemblyName = new AssemblyName(type.GetTypeInfo().Assembly.FullName);
            _localizer = factory.Create("EntityValidationResource", assemblyName.Name);
        }

        public LocalizedString this[string name] => throw new System.NotImplementedException();

        public LocalizedString this[string name, params object[] arguments] => throw new System.NotImplementedException();

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            throw new System.NotImplementedException();
        }

        public LocalizedString GetLocalizedHtmlString(string key)
        {
            return _localizer.GetString(key);
        }

        public IStringLocalizer WithCulture(CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }
    }
}

