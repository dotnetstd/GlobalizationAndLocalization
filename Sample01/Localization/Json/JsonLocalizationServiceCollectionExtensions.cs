﻿using System;

using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Localization;

using My.Extensions.Localization.Json;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class JsonLocalizationServiceCollectionExtensions
    {
        public static IServiceCollection AddJsonLocalization(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            services.AddOptions();
            AddJsonLocalizationServices(services);
            return services;
        }

        public static IServiceCollection AddJsonLocalization(this IServiceCollection services, Action<LocalizationOptions> setupAction)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (setupAction == null) throw new ArgumentNullException(nameof(setupAction));
            AddJsonLocalizationServices(services, setupAction);
            return services;
        }

        internal static void AddJsonLocalizationServices(IServiceCollection services)
        {
            services.TryAddSingleton<IStringLocalizerFactory, JsonStringLocalizerFactory>();
            services.TryAddTransient(typeof(IStringLocalizer<>), typeof(StringLocalizer<>));
        }

        internal static void AddJsonLocalizationServices(IServiceCollection services, Action<LocalizationOptions> setupAction)
        {
            AddJsonLocalizationServices(services);
            services.Configure(setupAction);
        }
    }
}
