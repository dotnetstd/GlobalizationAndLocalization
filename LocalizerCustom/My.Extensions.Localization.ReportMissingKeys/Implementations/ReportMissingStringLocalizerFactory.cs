using My.Extensions.Localization.ReportMissingKeys.Models;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;
using System.IO;

namespace My.Extensions.Localization.ReportMissingKeys.Implementations
{
    public class ReportMissingStringLocalizerFactory<T> : ReportMissingStringLocalizerFactory, IStringLocalizerFactory where T : IStringLocalizerFactory
    {
        public ReportMissingStringLocalizerFactory(IServiceProvider serviceProvider, IOptions<LocalizationOptions> localizationOptions)
            : base(serviceProvider.GetService(typeof(T)) as IStringLocalizerFactory, localizationOptions) { }
    }

    public class ReportMissingStringLocalizerFactory : IStringLocalizerFactory
    {
        private readonly IStringLocalizerFactory _underlayingFactory;

        private readonly ConcurrentDictionary<Tuple<string, string>, ReportMissingStringLocalizer> _localizerCache =
            new ConcurrentDictionary<Tuple<string, string>, ReportMissingStringLocalizer>();

        private readonly string _resourcesRelativePath;

        public ReportMissingStringLocalizerFactory(IStringLocalizerFactory factory, IOptions<LocalizationOptions> localizationOptions)
        {
            _underlayingFactory = factory;
            _resourcesRelativePath = localizationOptions.Value.ResourcesPath ?? string.Empty;
            if (!string.IsNullOrEmpty(_resourcesRelativePath))
            {
                _resourcesRelativePath += ".";
            }
        }

        public IStringLocalizer Create(Type resourceSource)
        {
            var localizer = _underlayingFactory.Create(resourceSource);
            return _localizerCache.GetOrAdd(
                new Tuple<string, string>(
                    resourceSource.GetTypeInfo().Assembly.GetName().Name,
                    resourceSource.Name
                ),
                new ReportMissingStringLocalizer(localizer)
            );
        }

        public IStringLocalizer Create(string baseName, string location)
        {
            var localizer = _underlayingFactory.Create(baseName, location);
            return _localizerCache.GetOrAdd(
                new Tuple<string, string>(
                    location,
                    baseName.Replace(location + ".", "")
                ),
                new ReportMissingStringLocalizer(localizer));
        }

        public MissingResourceKey[] GetMissingResources()
        {
            return _localizerCache.SelectMany(localizer =>
            {
                return localizer.Value.GetDiscoveredMissingKeys()
                    .Select(missing => new MissingResourceKey
                        {
                            Key = missing.Key,
                            Assembly = localizer.Key.Item1,
                            ResourceName = (_resourcesRelativePath + localizer.Key.Item2),
                            Cultures = missing.Value
                        }
                    );
            }).ToArray();
        }
    }
}