using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;

using OrchardCore.Localization;

using System.Collections.Generic;

namespace Sample01.Localization
{
    public class MyLIModularPoFileLocationProvider : ILocalizationFileLocationProvider
    {
        private string _resourcesContainer;

        public MyLIModularPoFileLocationProvider(IOptions<LocalizationOptions> localizationOptions)
        {
            _resourcesContainer = localizationOptions.Value.ResourcesPath; // Localization
        }
        public IEnumerable<IFileInfo> GetLocations(string cultureName)
        {
            return default;
        }
    }
}
