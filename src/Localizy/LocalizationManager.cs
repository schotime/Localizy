using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

namespace Localizy
{
    public static class LocalizationManager
    {
        private static LocalizationProvider _localizationProvider;

        public static CultureInfo DefaultCultureInfo { get; set; }

        public static LocalizationProvider Init(params ILocalizationStorageProvider[] localizationStorageProviders)
        {
            return Init(null, null, localizationStorageProviders);
        }

        public static LocalizationProvider Init(ILocalizationDataProvider localizationDataProvider, ILocalizationMissingHandler missingHandler, params ILocalizationStorageProvider[] localizationStorageProviders)
        {
            _localizationProvider = new LocalizationProvider(localizationDataProvider, missingHandler, localizationStorageProviders);
            return _localizationProvider;
        }

        public static IDictionary<LocalizationKey, string> GetStoredLocalizations(string name, CultureInfo cultureInfo)
        {
            return _localizationProvider.GetStoredLocalizations(name, cultureInfo);
        }

        public static void Reload(CultureInfo culture)
        {
            _localizationProvider.Reload(culture);
        }

        public static void Reload()
        {
            _localizationProvider.Reload();
        }

        public static string GetText(StringToken token, CultureInfo culture = null)
        {
            return _localizationProvider.GetText(token, culture);
        }

        public static string GetText(string key, CultureInfo culture = null)
        {
            return GetText(StringToken.FromKeyString(key), culture);
        }

        public static IEnumerable<StringToken> GetAllTokens(CultureInfo culture, Assembly assembly, Func<Type, bool> where)
        {
            return _localizationProvider.GetAllTokens(culture, assembly, where);
        }

        internal static CultureInfo GetCulture(CultureInfo culture)
        {
            return _localizationProvider.GetCulture(culture);
        }
    }
}
