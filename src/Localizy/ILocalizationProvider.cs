using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

namespace Localizy
{
    public interface ILocalizationProvider
    {
        IEnumerable<StringToken> GetAllTokens(CultureInfo culture, Assembly assembly, Func<Type, bool> where);
        IDictionary<LocalizationKey, string> GetStoredLocalizations(string name, CultureInfo cultureInfo);
        void Reload(CultureInfo cultureInfo);
        void Reload();
        string GetText(StringToken token);
        string GetText(StringToken token, CultureInfo culture);
        string GetText(StringToken token, object model);
        string GetText(StringToken token, CultureInfo culture, object model);
        string TryGetText(LocalizationKey key);
        string TryGetText(LocalizationKey key, CultureInfo culture);
        string TryGetText(LocalizationKey key, object model);
        string TryGetText(LocalizationKey key, CultureInfo culture, object model);
        void UpdateText(LocalizationKey localizationKey, CultureInfo culture, string value);
        CultureInfo GetCulture(CultureInfo culture);
    }
}