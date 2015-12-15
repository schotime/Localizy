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
        
        public IEnumerable<StringToken> GetAllTokens(CultureInfo culture, Assembly assembly, Func<Type, bool> where)
        {
            var stringTokens = assembly.GetExportedTypes().Where(where).SelectMany(x => x.RecurseNestedTypes()).SelectMany(ScanStringTokenType);
            return stringTokens;
        }

        private IEnumerable<StringToken> ScanStringTokenType(Type type)
        {
            return type.GetFields(BindingFlags.Static | BindingFlags.Public).Where(field => field.FieldType.CanBeCastTo<StringToken>()).Select(field => field.GetValue(null).As<StringToken>());
        }

        public string GetText(StringToken token, CultureInfo culture)
        {
            return FindTextViaHierarchy(token, culture);
        }

        private string FindTextViaHierarchy(StringToken token, CultureInfo culture)
        {
            var text = _localeCache[culture].Get(token.ToLocalizationKey(), () =>
            {
                if (culture.Parent == CultureInfo.InvariantCulture || culture == culture.Parent)
                {
                    return _missingHandler.FindMissingText(token, culture);
                }
                return FindTextViaHierarchy(token, culture.Parent);
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