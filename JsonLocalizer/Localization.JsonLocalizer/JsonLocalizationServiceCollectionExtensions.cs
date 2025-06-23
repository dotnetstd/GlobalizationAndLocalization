using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Localization;

using System;

namespace Microsoft.Extensions.DependencyInjection
{
	using global::Localization.JsonLocalizer;
	using global::Localization.JsonLocalizer.StringLocalizer;

	public static class JsonLocalizationServiceCollectionExtensions
	{
		public static IServiceCollection AddJsonLocalization2(this IServiceCollection services)
		{
			if (services == null)
			{
				throw new ArgumentNullException(nameof(services));
			}
			return AddJsonLocalization2(services, setupAction: null);
		}

		public static IServiceCollection AddJsonLocalization2(this IServiceCollection services, Action<JsonLocalizationOptions> setupAction)
		{
			if (services == null)
			{
				throw new ArgumentNullException(nameof(services));
			}

			services.TryAdd(new ServiceDescriptor(
				typeof(IStringLocalizerFactory),
				typeof(JsonStringLocalizerFactory),
				ServiceLifetime.Singleton));
			services.TryAdd(new ServiceDescriptor(
				typeof(IStringLocalizer),
				typeof(JsonStringLocalizer),
				ServiceLifetime.Singleton));

			if (setupAction != null)
			{
				services.Configure(setupAction);
			}
			return services;
		}
	}
}
