using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using System;
using System.Linq;
using System.Reflection;

namespace Sample01.Localization
{
    public class LocalizationOptions
    {
        public string ResourcesPath { get; set; } = string.Empty;

        public bool UseGenericResources { get; set; } = true;
    }
    public class LocalizationGenerics
    {
        private readonly bool _useGenericResources;

        public ResourceManagerStringLocalizerFactory(
            IOptions<LocalizationOptions> localizationOptions,
            ILoggerFactory loggerFactory)
        {
            if (localizationOptions == null)
            {
                throw new ArgumentNullException(nameof(localizationOptions));
            }

            if (loggerFactory == null)
            {
                throw new ArgumentNullException(nameof(loggerFactory));
            }

            _resourcesRelativePath = localizationOptions.Value.ResourcesPath ?? string.Empty;
            _useGenericResources = localizationOptions.Value.UseGenericResources;
            _loggerFactory = loggerFactory;

            if (!string.IsNullOrEmpty(_resourcesRelativePath))
            {
                _resourcesRelativePath = _resourcesRelativePath.Replace(Path.AltDirectorySeparatorChar, '.')
                    .Replace(Path.DirectorySeparatorChar, '.') + ".";
            }
        }

        public IStringLocalizer Create(Type resourceSource)
        {
            if (resourceSource == null)
            {
                throw new ArgumentNullException(nameof(resourceSource));
            }

            var typeInfo = resourceSource.GetTypeInfo();

            var baseName = GetResourcePrefix(typeInfo);
            if (!_useGenericResources)
            {
                if (baseName.Contains("`1"))
                {
                    var genericStartIndex = baseName.IndexOf("`1");
                    baseName = baseName.Substring(0, genericStartIndex);
                }
            }
            var assembly = typeInfo.Assembly;

            return _localizerCache.GetOrAdd(baseName, _ => CreateResourceManagerStringLocalizer(assembly, baseName));
        }

        public IStringLocalizer Create(string baseName, string location)
        {
            if (baseName == null)
            {
                throw new ArgumentNullException(nameof(baseName));
            }
        }

    }
}
