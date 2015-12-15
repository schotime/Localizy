using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

namespace Localizy
{
    public static class LocalizationManager
    {
        private static ILocalizationProvider _localizationProvider = Init();

        public static ILocalizationProvider Init(params ILocalizationStorageProvider[] localizationStorageProviders)
        {
            return Init(null, null, localizationStorageProviders);
        }

        public static ILocalizationProvider Init(ILocalizationDataProvider localizationDataProvider, ILocalizationMissingHandler missingHandler, params ILocalizationStorageProvider[] localizationStorageProviders)
        {
            _localizationProvider = new LocalizationProvider(localizationDataProvider, missingHandler, localizationStorageProviders);
            return _localizationProvider;
        }

        public static string GetText(StringToken token, CultureInfo culture = null)
        {
            return _localizationProvider.GetText(token, culture);
        }

        public static string GetText(string key, CultureInfo culture = null)
        {
            return GetText(StringToken.FromKeyString(key), culture);
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

        public static IEnumerable<StringToken> GetAllTokens(CultureInfo culture, Assembly assembly, Func<Type, bool> where)
        {
            return _localizationProvider.GetAllTokens(culture, assembly, where);
        }

        public static IDictionary<LocalizationKey, string> GetStoredLocalizations(string name, CultureInfo cultureInfo)
        {
            return _localizationProvider.GetStoredLocalizations(name, cultureInfo);
        }

        internal static CultureInfo GetCulture(CultureInfo culture)
        {
            return _localizationProvider.GetCulture(culture);
        }
    }
}
