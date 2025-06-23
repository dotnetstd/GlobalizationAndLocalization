using System;
using Microsoft.Extensions.Localization;

namespace MBODM.AspNetCore.SimpleLocalization
{
    public sealed class Localizer<TSharedResourceClass> : ILocalizer
    {
        private readonly IStringLocalizer<TSharedResourceClass> stringLocalizer;

        public Localizer(IStringLocalizer<TSharedResourceClass> stringLocalizer)
        {
            this.stringLocalizer = stringLocalizer ?? throw new ArgumentNullException(nameof(stringLocalizer));
        }

        public string this[string key]
        {
            get { return GetText(key); }
        }

        public string GetText(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("Argument is null or empty.", nameof(key));
            }

            return stringLocalizer[key];
        }
    }
}
