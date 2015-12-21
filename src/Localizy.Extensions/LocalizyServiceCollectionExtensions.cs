using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Localizy.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Localization;

namespace Localizy
{
    /// <summary>
    /// Extension methods for adding localization servics to the DI container.
    /// </summary>
    public static class LocalizyServiceCollectionExtensions
    {
        /// <summary>
        /// Adds services required for application localization.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
        /// <returns>The <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddLocalizy(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            return AddLocalizy(services, localizyOptions: null);
        }

        /// <summary>
        /// Adds services required for application localization.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
        /// <param name="localizyOptions">An <see cref="LocalizyOptions"/> object which contains the configuration</param>
        /// <returns>The <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddLocalizy(
            this IServiceCollection services,
            LocalizyOptions localizyOptions)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            var localizationProvider = new LocalizationProviderWithOptions(localizyOptions).WithFilter(localizyOptions.Filter);

            StringToken.LocalizationManager = new LocalizationManagerAdapter(localizationProvider);

            services.Add(new ServiceDescriptor(
                typeof (ILocalizationProvider),
                localizationProvider));

            //services.Add(new ServiceDescriptor(
            //    typeof(IStringLocalizer<>),
            //    typeof(LocalizyStringLocalizer<>),
            //    ServiceLifetime.Singleton));

            //services.Add(new ServiceDescriptor(
            //    typeof(IStringLocalizerFactory),
            //    typeof(LocalizyStringLocalizerFactory),
            //    ServiceLifetime.Transient));

            return services;
        }
    }
}
