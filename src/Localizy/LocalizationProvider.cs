using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Localizy
{
    public class LocalizationProvider : ILocalizationProvider
    {
        private readonly ILocalizationDataProvider _localizationDataProvider = new NulloLocalizationDataProvider();
        private IDictionary<string, Cache<CultureInfo, IDictionary<LocalizationKey, string>>> _localizationStorageCache = new Dictionary<string, Cache<CultureInfo, IDictionary<LocalizationKey, string>>>();
        private readonly Func<CultureInfo, ILocaleCache> _localeCacheFactory;
        private readonly ILocalizationStorageProvider[] _localizationStorageProviders;
        private readonly Lazy<IDictionary<LocalizationKey, StringToken>> _keyToTokens = new Lazy<IDictionary<LocalizationKey, StringToken>>();
        private Func<Type, bool> _filter = x => true;

        public Func<CultureInfo> CurrentCultureFactory { get; set; }

        public LocalizationProvider(Assembly assembly, params ILocalizationStorageProvider[] localizationStorageProviders) : this(new[] { assembly }, localizationStorageProviders)
        {
        }

        public LocalizationProvider(Assembly[] assemblies, params ILocalizationStorageProvider[] localizationStorageProviders) : this(assemblies, null, null, localizationStorageProviders)
        {
        }

        public LocalizationProvider(Assembly[] assemblies, ILocalizationDataProvider localizationDataProvider, ILocalizationMissingHandler missingHandler, params ILocalizationStorageProvider[] localizationStorageProviders)
        {
            _localizationStorageProviders = localizationStorageProviders;
            _localizationStorageCache = SetUpLocalizationStoreCache(localizationStorageProviders);
            _localeCacheFactory = x => new ThreadSafeLocaleCache(x, OverlayStoredLocations(x, localizationStorageProviders));
            _localizationDataProvider = localizationDataProvider ?? new LocalizationDataProvider(_localeCacheFactory, missingHandler ?? new DefaultValueLocalizationMissingHandler());
            _keyToTokens = new Lazy<IDictionary<LocalizationKey, StringToken>>(() => TokenScanner.GetAllTokens(assemblies, _filter).ToDictionary(x => x.ToLocalizationKey(), x => x));
        }

        public static LocalizationProvider Create<T>(params ILocalizationStorageProvider[] localizationStorageProviders)
        {
            return new LocalizationProvider(typeof(T).GetTypeInfo().Assembly, localizationStorageProviders);
        }

        public LocalizationProvider WithFilter(Func<Type, bool> filter)
        {
            _filter = filter;
            return this;
        }

        private IDictionary<string, Cache<CultureInfo, IDictionary<LocalizationKey, string>>> SetUpLocalizationStoreCache(ILocalizationStorageProvider[] localizationStorageProviders)
        {
            if (localizationStorageProviders.GroupBy(x => x.Name).Count() != localizationStorageProviders.Length)
                throw new Exception("LocalizationStorageProviders must have unique names");

            return localizationStorageProviders.ToDictionary(x => x.Name, x =>
            {
                return new Cache<CultureInfo, IDictionary<LocalizationKey, string>>(y => 
                {
                    var dict = new Dictionary<LocalizationKey, string>();
                    foreach (var item in x.Provide(y))
                    {
                        dict[item.Key] = item.Display;
                    }
                    return dict;
                });
            });
        }

        private IDictionary<LocalizationKey, string> OverlayStoredLocations(CultureInfo cultureInfo, ILocalizationStorageProvider[] localizationStorageProviders)
        {
            var dict = new Dictionary<LocalizationKey, string>();
            foreach (var localizationStorageProvider in localizationStorageProviders)
            {
                var provider = _localizationStorageCache[localizationStorageProvider.Name];
                foreach (var cultureValues in provider[cultureInfo])
                {
                    dict[cultureValues.Key] = cultureValues.Value;
                }
            }

            return dict;
        }

        public string TryGetText(LocalizationKey key)
        {
            return TryGetText(key, null);
        }

        public string TryGetText(LocalizationKey key, CultureInfo culture)
        {
            return TryGetText(key, culture, null);
        }

        public string TryGetText(LocalizationKey key, object model)
        {
            return TryGetText(key, null, model);
        }

        public string TryGetText(LocalizationKey key, CultureInfo culture, object model)
        {
            StringToken token;
            var isFound = _keyToTokens.Value.TryGetValue(key, out token);
            if (!isFound) { 
                return null;
            }
                
            return GetText(token, culture, model);
        }

        public string GetText(StringToken token)
        {
            return GetText(token, null);
        }

        public string GetText(StringToken token, CultureInfo culture)
        {
            return GetText(token, culture, null);
        }

        public string GetText(StringToken token, object model)
        {
            return GetText(token, null, model);
        }

        public string GetText(StringToken token, CultureInfo culture, object model)
        {
            return GetTextWithCulture(token, culture, model).Text;
        }

        public TextAndCulture GetTextWithCulture(StringToken token)
        {
            return GetTextWithCulture(token, null);
        }

        public TextAndCulture GetTextWithCulture(StringToken token, CultureInfo culture)
        {
            return GetTextWithCulture(token, culture, null);
        }

        public TextAndCulture GetTextWithCulture(StringToken token, object model)
        {
            return GetTextWithCulture(token, null, model);
        }

        public TextAndCulture GetTextWithCulture(StringToken token, CultureInfo culture, object model)
        {
            var result = _localizationDataProvider.GetTextWithCulture(token, GetCulture(culture));
            if (result != null && model != null)
            {
                result.Text = ObjectFormatter.TokenFormat(result.Text, model);
            }
            return result;
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
                cache.Value.Remove(cultureInfo);
            }

            _localizationDataProvider.Reload(cultureInfo, _localeCacheFactory);
        }

        public IDictionary<LocalizationKey, string> GetStoredLocalizations(string name, CultureInfo cultureInfo)
        {
            return _localizationStorageCache[name][cultureInfo];
        }

        public CultureInfo GetCulture(CultureInfo culture)
        {
            return culture ?? (CurrentCultureFactory != null ? CurrentCultureFactory() : null) ??
#if DOTNET54
            CultureInfo.CurrentUICulture;
#else
            System.Threading.Thread.CurrentThread.CurrentUICulture;
#endif
        }

        public IDictionary<LocalizationKey, StringToken> GetAllTokens()
        {
            return _keyToTokens.Value;
        }
    }
}