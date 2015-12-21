using System.Reflection;
using Microsoft.Extensions.Configuration;
using System;
using System.Globalization;

namespace Localizy
{
    public class LocalizyOptions
    {
        public Assembly[] Assemblies { get; }
        public ILocalizationStorageProvider[] LocalizationStorageProviders { get; }
        public ILocalizationMissingHandler LocalizationMissingHandler { set; get; }
        public ILocalizationDataProvider LocalizationDataProvider { get; set; }
        public Func<Type, bool> Filter { get; set; } = x => true;
        public Func<CultureInfo> CurrentCultureFactory { get; set; }

        public static LocalizyOptions From<T>(Action<LocalizyOptions> modifier = null, params ILocalizationStorageProvider[] localizationStorageProviders)
        {
            var options = new LocalizyOptions(new[] { typeof(T).GetTypeInfo().Assembly }, localizationStorageProviders);
            modifier?.Invoke(options);
            return options;
        }

        public LocalizyOptions(Assembly assembly, params ILocalizationStorageProvider[] localizationStorageProviders) :
            this(new[] { assembly }, localizationStorageProviders)
        {
        }

        public LocalizyOptions(Assembly[] assemblies, params ILocalizationStorageProvider[] localizationStorageProviders)
        {
            Assemblies = assemblies;
            LocalizationStorageProviders = localizationStorageProviders;
        }
    }
}