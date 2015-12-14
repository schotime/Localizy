using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Localizy.Storage
{
    public class InMemoryLocalizationStorageProvider : ILocalizationStorageProvider
    {
        private readonly Dictionary<CultureInfo, List<LocalString>> _data;

        public InMemoryLocalizationStorageProvider(string name, Dictionary<CultureInfo, List<LocalString>> data)
        {
            Name = name;
            _data = data;
        }

        public string Name { get; private set; }

        public IEnumerable<LocalString> Provide(CultureInfo culture)
        {
            if (!_data.ContainsKey(culture))
                return Enumerable.Empty<LocalString>();
            return _data[culture].Select(x => new LocalString(x.Key, x.Display));
        }
    }
}