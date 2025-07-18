using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Localization.Internal;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Resources;

namespace AddingLocalization.Services
{
    /// <summary>
    /// An <see cref="IStringLocalizer"/> that uses the <see cref="ResourceManager"/> and
    /// <see cref="ResourceReader"/> to provide localized strings.
    /// </summary>
    /// <remarks>This type is thread-safe.</remarks>
    public class SingleFileResourceManagerStringLocalizer : IStringLocalizer
    {
        private readonly ConcurrentDictionary<string, object> _missingManifestCache = new ConcurrentDictionary<string, object>();
        private readonly IResourceNamesCache _resourceNamesCache;
        private readonly string _keyPrefix;
        private readonly ResourceManager _resourceManager;
        private readonly IResourceStringProvider _resourceStringProvider;
        private readonly string _resourceBaseName;

        /// <summary>
        /// Creates a new <see cref="SingleFileResourceManagerStringLocalizer"/>.
        /// </summary>
        /// <param name="resourceManager">The <see cref="ResourceManager"/> to read strings from.</param>
        /// <param name="resourceAssembly">The <see cref="Assembly"/> that contains the strings as embedded resources.</param>
        /// <param name="baseName">The base name of the embedded resource that contains the strings.</param>
        /// <param name="resourceNamesCache">Cache of the list of strings for a given resource assembly name.</param>
        public SingleFileResourceManagerStringLocalizer(
            ResourceManager resourceManager,
            Assembly resourceAssembly,
            string baseName,
            IResourceNamesCache resourceNamesCache,
            string keyPrefix)
            : this(
                resourceManager,
                new AssemblyResourceStringProvider(
                    resourceNamesCache,
                    new AssemblyWrapper(resourceAssembly),
                    baseName),
                baseName,
                resourceNamesCache,
                keyPrefix)
        {
            if (resourceAssembly == null)
            {
                throw new ArgumentNullException(nameof(resourceAssembly));
            }
        }

        /// <summary>
        /// Intended for testing purposes only.
        /// </summary>
        public SingleFileResourceManagerStringLocalizer(
            ResourceManager resourceManager,
            IResourceStringProvider resourceStringProvider,
            string baseName,
            IResourceNamesCache resourceNamesCache,
            string keyPrefix)
        {
            if (resourceManager == null)
            {
                throw new ArgumentNullException(nameof(resourceManager));
            }

            if (resourceStringProvider == null)
            {
                throw new ArgumentNullException(nameof(resourceStringProvider));
            }

            if (baseName == null)
            {
                throw new ArgumentNullException(nameof(baseName));
            }

            if (resourceNamesCache == null)
            {
                throw new ArgumentNullException(nameof(resourceNamesCache));
            }

            _resourceStringProvider = resourceStringProvider;
            _resourceManager = resourceManager;
            _resourceBaseName = baseName;
            _resourceNamesCache = resourceNamesCache;
            _keyPrefix = keyPrefix;
        }

        /// <inheritdoc />
        public virtual LocalizedString this[string name]
        {
            get
            {
                if (name == null)
                {
                    throw new ArgumentNullException(nameof(name));
                }

                var value = GetStringSafely(name, null);
                return new LocalizedString(name, value ?? name, resourceNotFound: value == null);
            }
        }

        /// <inheritdoc />
        public virtual LocalizedString this[string name, params object[] arguments]
        {
            get
            {
                if (name == null)
                {
                    throw new ArgumentNullException(nameof(name));
                }

                var format = GetStringSafely(name, null);
                var value = string.Format(format ?? name, arguments);
                return new LocalizedString(name, value, resourceNotFound: format == null);
            }
        }

        /// <summary>
        /// Creates a new <see cref="SingleFileResourceManagerStringLocalizer"/> for a specific <see cref="CultureInfo"/>.
        /// </summary>
        /// <param name="culture">The <see cref="CultureInfo"/> to use.</param>
        /// <returns>A culture-specific <see cref="SingleFileResourceManagerStringLocalizer"/>.</returns>
        public IStringLocalizer WithCulture(CultureInfo culture)
        {
            return culture == null
                ? new SingleFileResourceManagerStringLocalizer(
                    _resourceManager,
                    _resourceStringProvider,
                    _resourceBaseName,
                    _resourceNamesCache,
                    _keyPrefix)
                : new SingleFileResourceManagerWithCultureStringLocalizer(
                    _resourceManager,
                    _resourceStringProvider,
                    _resourceBaseName,
                    _resourceNamesCache,
                    culture,
                    _keyPrefix);
        }

        /// <inheritdoc />
        public virtual IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures) =>
            GetAllStrings(includeParentCultures, CultureInfo.CurrentUICulture);

        /// <summary>
        /// Returns all strings in the specified culture.
        /// </summary>
        /// <param name="includeParentCultures"></param>
        /// <param name="culture">The <see cref="CultureInfo"/> to get strings for.</param>
        /// <returns>The strings.</returns>
        protected IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures, CultureInfo culture)
        {
            if (culture == null)
            {
                throw new ArgumentNullException(nameof(culture));
            }

            var resourceNames = includeParentCultures
                ? GetResourceNamesFromCultureHierarchy(culture)
                : _resourceStringProvider.GetAllResourceStrings(culture, true);

            foreach (var name in resourceNames)
            {
                var value = GetStringSafely(name, culture);
                yield return new LocalizedString(name, value ?? name, resourceNotFound: value == null);
            }
        }

        /// <summary>
        /// Gets a resource string from the <see cref="_resourceManager"/> and returns <c>null</c> instead of
        /// throwing exceptions if a match isn't found.
        /// </summary>
        /// <param name="name">The name of the string resource.</param>
        /// <param name="culture">The <see cref="CultureInfo"/> to get the string for.</param>
        /// <returns>The resource string, or <c>null</c> if none was found.</returns>
        protected string GetStringSafely(string name, CultureInfo culture)
        {
            name = _keyPrefix + "." + name;
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            var cacheKey = $"name={name}&culture={(culture ?? CultureInfo.CurrentUICulture).Name}";

            if (_missingManifestCache.ContainsKey(cacheKey))
            {
                return null;
            }

            try
            {
                return culture == null ? _resourceManager.GetString(name) : _resourceManager.GetString(name, culture);
            }
            catch (MissingManifestResourceException)
            {
                _missingManifestCache.TryAdd(cacheKey, null);
                return null;
            }
        }

        private IEnumerable<string> GetResourceNamesFromCultureHierarchy(CultureInfo startingCulture)
        {
            var currentCulture = startingCulture;
            var resourceNames = new HashSet<string>();

            var hasAnyCultures = false;

            while (true)
            {

                var cultureResourceNames = _resourceStringProvider.GetAllResourceStrings(currentCulture, false);

                if (cultureResourceNames != null)
                {
                    foreach (var resourceName in cultureResourceNames)
                    {
                        resourceNames.Add(resourceName);
                    }
                    hasAnyCultures = true;
                }

                if (currentCulture == currentCulture.Parent)
                {
                    // currentCulture begat currentCulture, probably time to leave
                    break;
                }

                currentCulture = currentCulture.Parent;
            }

            if (!hasAnyCultures)
            {
                throw new MissingManifestResourceException("No manifests exist for the current culture");
            }

            return resourceNames;
        }
    }
}