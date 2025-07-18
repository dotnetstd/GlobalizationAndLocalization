
using MainApp.Models.Localization;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Localization;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Sample01.Localization
{
    public class InMemoryStringLocalizerFactory : IStringLocalizerFactory
    {
        private readonly IList<Resource> _resources;

        public InMemoryStringLocalizerFactory(
            IList<Resource> resources)
        {
            _resources = resources ?? throw new ArgumentNullException(nameof(resources));
        }

        public IStringLocalizer Create(Type resourceSource)
            => new InMemoryStringLocalizer(_resources);

        public IStringLocalizer Create(string baseName, string location)
            => new InMemoryStringLocalizer(_resources);
    }

    public class InMemoryStringLocalizer : IStringLocalizer
    {
        private readonly CultureInfo _culture;
        private readonly IList<Resource> _resources;

        public InMemoryStringLocalizer(
            IList<Resource> resources)
        {
            _resources = resources ?? throw new ArgumentNullException(nameof(resources));
        }

        public InMemoryStringLocalizer(
            IList<Resource> resources,
            CultureInfo culture)
        {
            _resources = resources ?? throw new ArgumentNullException(nameof(resources));
            _culture = culture ?? throw new ArgumentNullException(nameof(culture));
        }

        public virtual LocalizedString this[string name]
        {
            get
            {
                if (name == null)
                {
                    throw new ArgumentNullException(nameof(name));
                }

                var culture = _culture ?? CultureInfo.CurrentUICulture;
                var value = _resources.SingleOrDefault(r => r.Name == culture.Name && r.Key == name)?.Value;

                return new LocalizedString(name, value ?? name, resourceNotFound: value == null);
            }
        }

        public virtual LocalizedString this[string name, params object[] arguments]
        {
            get
            {
                if (name == null)
                {
                    throw new ArgumentNullException(nameof(name));
                }

                var culture = _culture ?? CultureInfo.CurrentUICulture;
                var format = _resources.SingleOrDefault(r => r.Name == culture.Name && r.Key == name)?.Value;
                var value = string.Format(format ?? name, arguments);

                return new LocalizedString(name, value, resourceNotFound: format == null);
            }
        }

        public IStringLocalizer WithCulture(CultureInfo culture)
            => new InMemoryStringLocalizer(_resources, culture);

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
            => _resources.Select(r => new LocalizedString(r.Key, r.Value, true)).ToList();
    }
    public static class InMemoryClass
    {
        public static IServiceCollection AddInMemoryLocalization(this IServiceCollection services, IList<Resource> resources)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (resources == null)
            {
                throw new ArgumentNullException(nameof(resources));
            }

            services.TryAddSingleton<IStringLocalizerFactory>(new InMemoryStringLocalizerFactory(resources));
            services.TryAddTransient(typeof(IStringLocalizer<>), typeof(StringLocalizer<>));

            return services;
        }
    }
}