using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;

namespace Localizy
{
    public class ThreadSafeLocaleCache : ILocaleCache
    {
        private readonly CultureInfo _culture;
        private readonly IDictionary<LocalizationKey, string> _data;
        private readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();

        public ThreadSafeLocaleCache(CultureInfo culture, IEnumerable<LocalString> strings)
        {
            _data = new Dictionary<LocalizationKey, string>();
            strings.Each(s =>
            {
				var localizationKey = new LocalizationKey(s.Key);
				if (_data.ContainsKey(localizationKey))
				{
					throw new ArgumentException(string.Format("Could not add localization key '{0}' to the cache as it already exists.", s.Key));
				}
				
				_data.Add(localizationKey, s.Display);
            });

            _culture = culture;
        }

        public ThreadSafeLocaleCache(CultureInfo culture, IDictionary<LocalizationKey, string> data)
        {
            _culture = culture;
            _data = data;
        }

        public CultureInfo Culture { get { return _culture; } }
        
        public void Update(LocalizationKey key, string value)
        {
            _lock.Write(() =>
            {
                if (_data.ContainsKey(key))
                {
                    _data[key] = value;
                }
                else
                {
                    _data.Add(key, value);
                }
            });
        }

        public string Get(LocalizationKey key, Func<string> missing)
        {
            var text = InitialRead(key);

            if (text == null)
            {
                text = missing();
                Update(key, text);
            }

            return text;
        }

        private string InitialRead(LocalizationKey key)
        {
            return _lock.Read(() => !_data.ContainsKey(key) ? null : _data[key]);
        }
    }
}