using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Localizy
{
    public class LocalizationDataProvider : ILocalizationDataProvider
    {
        private Cache<CultureInfo, ILocaleCache> _localeCache;
        private readonly ILocalizationMissingHandler _missingHandler;
        private Func<CultureInfo, ILocaleCache> _localeCacheFactory;

        public LocalizationDataProvider(Func<CultureInfo, ILocaleCache> localeCacheFactory, ILocalizationMissingHandler missingHandler)
        {
            _localeCacheFactory = localeCacheFactory;
            _localeCache = new Cache<CultureInfo, ILocaleCache>(localeCacheFactory);
            _missingHandler = missingHandler;
        }
        
        public string GetText(StringToken token, CultureInfo culture, Func<string> missingFunc = null)
        {
            return FindTextViaHierarchy(token, culture, missingFunc);
        }

        private string FindTextViaHierarchy(StringToken token, CultureInfo culture, Func<string> missingFunc)
        {
            var text = _localeCache[culture].Get(token.ToLocalizationKey(), () =>
            {
                if (culture.Parent == CultureInfo.InvariantCulture || culture == culture.Parent)
                {
                    return missingFunc != null ? missingFunc() : _missingHandler.FindMissingText(token, culture);
                }
                return FindTextViaHierarchy(token, culture.Parent, missingFunc);
            });
            
            return text;
        }

        public void UpdateText(LocalizationKey key, CultureInfo culture, string value)
        {
            _localeCache[culture].Update(key, value);
        }

        public void Reload()
        {
            _localeCache = new Cache<CultureInfo, ILocaleCache>(_localeCacheFactory);
        }

        public void Reload(CultureInfo cultureInfo, Func<CultureInfo, ILocaleCache> factory)
        {
            _localeCache[cultureInfo] = factory(cultureInfo);
        }
    }
}