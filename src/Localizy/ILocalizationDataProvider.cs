using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

namespace Localizy
{
    public interface ILocalizationDataProvider
    {
        string GetText(StringToken key, CultureInfo culture);
        void UpdateText(LocalizationKey key, CultureInfo culture, string value);
        void Reload(CultureInfo culture, Func<CultureInfo, ILocaleCache> factory);
        void Reload();
        IEnumerable<StringToken> GetAllTokens(CultureInfo culture, Assembly assembly, Func<Type, bool> where);
    }
}