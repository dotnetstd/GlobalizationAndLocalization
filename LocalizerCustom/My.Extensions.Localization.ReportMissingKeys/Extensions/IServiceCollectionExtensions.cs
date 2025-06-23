using System;
using System.Linq;
using System.Reflection;
using My.Extensions.Localization.ReportMissingKeys.Implementations;
using My.Extensions.Localization.ReportMissingKeys.Interfaces;
using My.Extensions.Localization.ReportMissingKeys.Options;
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Builder;
using My.Extensions.Localization.ReportMissingKeys.Middlewares;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection ReportMissingKeys(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            AddReportingServices(services);
            return services;
        }

        public static IServiceCollection ReportMissingKeys(this IServiceCollection services, Action<ReportOptions> setupAction)
        {

            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (setupAction == null)
            {
                throw new ArgumentNullException(nameof(setupAction));
            }

            AddReportingServices(services);
            services.Configure(setupAction);
            return services;
        }

        internal static void AddReportingServices(IServiceCollection services)
        {
            var originalServices = services.Where(x => x.ServiceType == typeof(IStringLocalizerFactory)).ToList();

            if (originalServices.Count() == 0)
            {
                throw new Exception("You must first configure the Localization service");
            }

            foreach (var originalService in originalServices)
            {
                services.Remove(originalService);

                var injectedReporterType = typeof(ReportMissingStringLocalizerFactory<>)
                    .GetTypeInfo()
                    .MakeGenericType(originalService.ImplementationType);

                services.AddSingleton(typeof(IStringLocalizerFactory), injectedReporterType)
                        .Add(new ServiceDescriptor(
                                originalService.ImplementationType,
                                originalService.ImplementationType,
                                originalService.Lifetime
                            )
                        );
            }
        }

        public static IServiceCollection ReportMissingAsResxManagerImport(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            return services.AddSingleton<IOutputFormatter, ResxManagerOutputFormatter>();
        }

        public static IApplicationBuilder UseReportingMissingKeys(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ReportMissingMiddleware>();
        }
    }
}