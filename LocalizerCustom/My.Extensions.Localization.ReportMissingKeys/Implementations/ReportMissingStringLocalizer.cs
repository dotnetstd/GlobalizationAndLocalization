using My.Extensions.Localization.ReportMissingKeys.Models;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Globalization;
using System.Collections.Concurrent;
using System;

namespace My.Extensions.Localization.ReportMissingKeys.Implementations
{
    public class ReportMissingStringLocalizer : IStringLocalizer
    {
        private readonly CultureInfo _culture;

        private readonly IStringLocalizer _underlayingLocalizer;

        private readonly ConcurrentDictionary<Tuple<string, string>, LocalizedString> _missingKeyCache;

        public ReportMissingStringLocalizer(IStringLocalizer underlayingLocalizer)
            : this(underlayingLocalizer, null, null)
        { }

        public ReportMissingStringLocalizer(IStringLocalizer underlayingLocalizer, CultureInfo culture) : this(underlayingLocalizer, culture, null)
        { }

        private ReportMissingStringLocalizer(IStringLocalizer underlayingLocalizer, CultureInfo culture, ConcurrentDictionary<Tuple<string, string>, LocalizedString> missingKeyCache)
        {
            if (underlayingLocalizer == null)
            {
                throw new ArgumentNullException(nameof(underlayingLocalizer));
            }

            _culture = culture;
            _underlayingLocalizer = underlayingLocalizer;
            _missingKeyCache = missingKeyCache ?? new ConcurrentDictionary<Tuple<string, string>, LocalizedString>();
        }

        public LocalizedString this[string name]
        {
            get
            {
                var value = _underlayingLocalizer[name];
                if (value.ResourceNotFound)
                {
                    _missingKeyCache.TryAdd(new Tuple<string, string>(name, (_culture ?? CultureInfo.CurrentUICulture).TwoLetterISOLanguageName), value);
                }
                return value;
            }
        }

        public LocalizedString this[string name, params object[] arguments]
        {
            get
            {
                var value = _underlayingLocalizer[name, arguments];
                if (value.ResourceNotFound)
                {
                    _missingKeyCache.TryAdd(new Tuple<string, string>(name, (_culture ?? CultureInfo.CurrentUICulture).TwoLetterISOLanguageName), value);
                }
                return value;
            }
        }

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            return _underlayingLocalizer.GetAllStrings(includeParentCultures);
        }

        public IStringLocalizer WithCulture(CultureInfo culture)
        {
            return new ReportMissingStringLocalizer(_underlayingLocalizer.WithCulture(culture), culture, _missingKeyCache);
        }

        internal Dictionary<string, string[]> GetDiscoveredMissingKeys()
        {
            return _missingKeyCache.GroupBy(x => x.Key.Item1).ToDictionary(
                k => k.Key,
                v => v.Select(x => x.Key.Item2).ToArray()
            );
        }
    }
}