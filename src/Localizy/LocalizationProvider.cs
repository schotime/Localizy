using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace Localizy
{
    public class LocalizationProvider
    {
        private readonly ILocalizationDataProvider _localizationDataProvider = new NulloLocalizationDataProvider();
        private Cache<string, Cache<CultureInfo, List<LocalString>>> _localizationStorageCache = new Cache<string, Cache<CultureInfo, List<LocalString>>>();
        private readonly Func<CultureInfo, ILocaleCache> _localeCacheFactory;
        private readonly ILocalizationStorageProvider[] _localizationStorageProviders;

        public Func<CultureInfo> CurrentCultureFactory { get; set; }

        public LocalizationProvider(params ILocalizationStorageProvider[] localizationStorageProviders)
            : this(null, null, localizationStorageProviders)
        {
        }

        public LocalizationProvider(ILocalizationDataProvider localizationDataProvider, ILocalizationMissingHandler missingHandler, params ILocalizationStorageProvider[] localizationStorageProviders)
        {
            _localizationStorageProviders = localizationStorageProviders;
            _localizationStorageCache = SetUpLocalizationStoreCache(localizationStorageProviders);
            _localeCacheFactory = x => new ThreadSafeLocaleCache(x, OverlayStoredLocations(x, localizationStorageProviders));
            _localizationDataProvider = localizationDataProvider
                ?? new LocalizationDataProvider(_localeCacheFactory, missingHandler ?? new DefaultValueLocalizationMissingHandler());
        }

        private Cache<string, Cache<CultureInfo, List<LocalString>>> SetUpLocalizationStoreCache(ILocalizationStorageProvider[] localizationStorageProviders)
        {
            if (localizationStorageProviders.GroupBy(x => x.Name).Count() != localizationStorageProviders.Length)
                throw new Exception("LocalizationStorageProviders must have unique names");

            var cache = new Cache<string, Cache<CultureInfo, List<LocalString>>>(x =>
            {
                var list = localizationStorageProviders.SingleOrDefault(z => z.Name == x);
                return new Cache<CultureInfo, List<LocalString>>(y => list != null ? list.Provide(y).ToList() : new List<LocalString>());
            });

            return cache;
        }

        private IDictionary<LocalizationKey, string> OverlayStoredLocations(CultureInfo cultureInfo, ILocalizationStorageProvider[] localizationStorageProviders)
        {
            var dict = new Dictionary<LocalizationKey, string>();
            foreach (var localizationStorageProvider in localizationStorageProviders)
            {
                var provider = _localizationStorageCache[localizationStorageProvider.Name];
                foreach (var localString in provider[cultureInfo])
                {
                    dict[new LocalizationKey(localString.Key)] = localString.Display;
                }
            }
            return dict;
        }

        public string GetText(StringToken token, CultureInfo culture = null)
        {
            return _localizationDataProvider.GetText(token, GetCulture(culture));
        }

        public string GetText(string key, CultureInfo culture = null)
        {
            return GetText(StringToken.FromKeyString(key), GetCulture(culture));
        }

        public void UpdateText(LocalizationKey localizationKey, CultureInfo culture, string value)
        {
            _localizationDataProvider.UpdateText(localizationKey, culture, value);
        }

        public void Reload()
        {
            _localizationStorageCache = SetUpLocalizationStoreCache(_localizationStorageProviders);
            _localizationDataProvider.Reload();
        }

        public void Reload(CultureInfo cultureInfo)
        {
            foreach (var cache in _localizationStorageCache)
            {
                cache.Remove(cultureInfo);
            }

            _localizationDataProvider.Reload(cultureInfo, _localeCacheFactory);
        }

        public IDictionary<LocalizationKey, string> GetStoredLocalizations(string name, CultureInfo cultureInfo)
        {
            return _localizationStorageCache[name][cultureInfo].ToDictionary(x => new LocalizationKey(x.Key), x => x.Display);
        }

        public IEnumerable<StringToken> GetAllTokens(CultureInfo culture, Assembly assembly, Func<Type, bool> where)
        {
            return _localizationDataProvider.GetAllTokens(culture, assembly, where);
        }

        internal CultureInfo GetCulture(CultureInfo culture)
        {
            return culture ?? (CurrentCultureFactory != null ? CurrentCultureFactory() : null) ??
#if DOTNET54
            CultureInfo.CurrentUICulture;
#else
            Thread.CurrentThread.CurrentUICulture;
#endif            
        }
    }
}