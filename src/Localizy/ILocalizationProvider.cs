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
        string GetText(StringToken token, CultureInfo culture = null);
        string GetText(string key, CultureInfo culture = null);
        void UpdateText(LocalizationKey localizationKey, CultureInfo culture, string value);
        CultureInfo GetCulture(CultureInfo culture);
    }
}