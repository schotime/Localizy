using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Localizy
{
    public class LocalizationDataProvider : ILocalizationDataProvider
    {
        private readonly Cache<CultureInfo, ILocaleCache> _localeCache;
        private readonly ILocalizationMissingHandler _missingHandler;
        private readonly Func<CultureInfo, ILocaleCache> _localeCacheFactory;

        public LocalizationDataProvider(Func<CultureInfo, ILocaleCache> localeCacheFactory, ILocalizationMissingHandler missingHandler)
        {
            _localeCacheFactory = localeCacheFactory;
            _localeCache = new Cache<CultureInfo, ILocaleCache>(localeCacheFactory);
            _missingHandler = missingHandler;
        }
        
        public TextAndCulture GetTextWithCulture(StringToken token, CultureInfo culture)
        {
            return FindTextViaHierarchy(token, culture);
        }

        private TextAndCulture FindTextViaHierarchy(StringToken token, CultureInfo culture)
        {
            var text = _localeCache[culture].Get(token.ToLocalizationKey(), () =>
            {
                if (culture.Parent == CultureInfo.InvariantCulture || culture == culture.Parent)
                {
                    return _missingHandler.FindMissingText(token, culture);
                }
                return null;
            });

            if (text == null && (culture.Parent != CultureInfo.InvariantCulture && culture != culture.Parent))
            {
                return FindTextViaHierarchy(token, culture.Parent);
            }

            return new TextAndCulture(text, culture);
        }

        public void UpdateText(LocalizationKey key, CultureInfo culture, string value)
        {
            _localeCache[culture].Update(key, value);
        }

        public void Reload()
        {
            _localeCache.ClearAll();
        }

        public void Reload(CultureInfo cultureInfo, Func<CultureInfo, ILocaleCache> factory)
        {
            _localeCache[cultureInfo] = factory(cultureInfo);
        }
    }
}