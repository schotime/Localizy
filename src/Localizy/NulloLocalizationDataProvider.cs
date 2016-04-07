using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Threading;

namespace Localizy
{
    public class NulloLocalizationDataProvider : ILocalizationDataProvider
    {
        public string GetText(StringToken key, CultureInfo culture = null)
        {
#if DOTNET54
            culture = culture ?? CultureInfo.CurrentUICulture;
#else
            culture = culture ?? Thread.CurrentThread.CurrentUICulture;
#endif
            return key.DefaultValue ?? culture.Name + "_" + key.Key;
        }

        public TextAndCulture GetTextWithCulture(StringToken key, CultureInfo culture)
        {
            return new TextAndCulture(GetText(key, culture), culture);
        }

        public void UpdateText(LocalizationKey key, CultureInfo culture, string value)
        {
            
        }

        public void Reload(CultureInfo cultureInfo, Func<CultureInfo, ILocaleCache> factory)
        {
            
        }

        public void Reload()
        {
            
        }

        public IEnumerable<StringToken> GetAllTokens(CultureInfo culture, Assembly assembly, Func<Type, bool> where)
        {
            yield break;
        }
    }
}