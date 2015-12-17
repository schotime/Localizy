using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

namespace Localizy
{
    public interface ILocalizationDataProvider
    {
        string GetText(StringToken key, CultureInfo culture, Func<string> missingFunc = null);
        void UpdateText(LocalizationKey key, CultureInfo culture, string value);
        void Reload(CultureInfo culture, Func<CultureInfo, ILocaleCache> factory);
        void Reload();
    }
}