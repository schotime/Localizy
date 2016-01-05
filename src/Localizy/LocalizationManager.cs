using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

namespace Localizy
{
    public static class LocalizationManager
    {
        private static ILocalizationProvider _localizationProvider = Init(new Assembly[0]);
        private static bool initialized = false;

        public static ILocalizationProvider InitSafe(Assembly assembly, params ILocalizationStorageProvider[] localizationStorageProviders)
        {
            return InitSafe(new[] { assembly }, localizationStorageProviders);
        }

        public static ILocalizationProvider InitSafe<T>(params ILocalizationStorageProvider[] localizationStorageProviders)
        {
            return InitSafe(new[] { typeof(T).GetTypeInfo().Assembly }, localizationStorageProviders);
        }

        public static ILocalizationProvider InitSafe(Assembly[] assemblies, params ILocalizationStorageProvider[] localizationStorageProviders)
        {
            return InitSafe(assemblies, null, null, localizationStorageProviders);
        }

        public static ILocalizationProvider InitSafe(Assembly[] assemblies, ILocalizationDataProvider localizationDataProvider, ILocalizationMissingHandler missingHandler, params ILocalizationStorageProvider[] localizationStorageProviders)
        {
            _localizationProvider = initialized ? _localizationProvider : Init(assemblies, localizationDataProvider, missingHandler, localizationStorageProviders);
            return _localizationProvider;
        }

        public static ILocalizationProvider Init(Assembly assembly, params ILocalizationStorageProvider[] localizationStorageProviders)
        {
            return Init(new[] { assembly }, localizationStorageProviders);
        }

        public static ILocalizationProvider Init<T>(params ILocalizationStorageProvider[] localizationStorageProviders)
        {
            return Init(new[] { typeof(T).GetTypeInfo().Assembly }, localizationStorageProviders);
        }

        public static ILocalizationProvider Init(Assembly[] assemblies, params ILocalizationStorageProvider[] localizationStorageProviders)
        {
            return Init(assemblies, null, null, localizationStorageProviders);
        }

        public static ILocalizationProvider Init(Assembly[] assemblies, ILocalizationDataProvider localizationDataProvider, ILocalizationMissingHandler missingHandler, params ILocalizationStorageProvider[] localizationStorageProviders)
        {
            _localizationProvider = new LocalizationProvider(assemblies, localizationDataProvider, missingHandler, localizationStorageProviders);
            initialized = true;
            return _localizationProvider;
        }

        public static string GetText(StringToken token, CultureInfo culture = null, object model = null)
        {
            return _localizationProvider.GetText(token, culture, model);
        }

        public static string TryGetText(LocalizationKey key, CultureInfo culture = null, object model = null)
        {
            return _localizationProvider.TryGetText(key, culture, model);
        }

        public static void UpdateText(LocalizationKey key, CultureInfo culture, string value)
        {
            _localizationProvider.UpdateText(key, culture, value);
        }

        public static void Reload(CultureInfo culture)
        {
            _localizationProvider.Reload(culture);
        }

        public static void Reload()
        {
            _localizationProvider.Reload();
        }

        public static IDictionary<LocalizationKey, StringToken> GetAllTokens()
        {
            return _localizationProvider.GetAllTokens();
        }

        public static IDictionary<LocalizationKey, string> GetStoredLocalizations(string name, CultureInfo cultureInfo)
        {
            return _localizationProvider.GetStoredLocalizations(name, cultureInfo);
        }
    }
}
