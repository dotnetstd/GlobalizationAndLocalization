using System;
using Microsoft.Extensions.DependencyInjection;

namespace MBODM.AspNetCore.SimpleLocalization
{
    public static class SimpleLocalizationMiddleware
    {
        public static IServiceCollection AddSimpleLocalization<TSharedResourceClass>(
            this IServiceCollection serviceCollection, string resourcesPath)
            where TSharedResourceClass : class
        {
            if (serviceCollection == null)
            {
                throw new ArgumentNullException(nameof(serviceCollection));
            }

            if (string.IsNullOrEmpty(resourcesPath))
            {
                throw new ArgumentException("Argument is null or empty.", nameof(resourcesPath));
            }

            return serviceCollection.
                AddScoped<ILocalizer, Localizer<TSharedResourceClass>>().
                AddLocalization(options => options.ResourcesPath = resourcesPath);
        }

        public static IServiceCollection AddSimpleLocalization<TSharedResourceClass>(
            this IServiceCollection serviceCollection)
            where TSharedResourceClass : class
        {
            return serviceCollection.AddSimpleLocalization<TSharedResourceClass>("Resources");
        }

        public static IMvcBuilder AddSimpleLocalizationForDataAnnotations<TSharedResourceClass>(
            this IMvcBuilder mvcBuilder)
            where TSharedResourceClass : class
        {
            if (mvcBuilder == null)
            {
                throw new ArgumentNullException(nameof(mvcBuilder));
            }

            return mvcBuilder.AddDataAnnotationsLocalization(options =>
            {
                options.DataAnnotationLocalizerProvider = (type, factory) =>
                {
                    return factory.Create(typeof(TSharedResourceClass));
                };
            });
        }
    }
}
