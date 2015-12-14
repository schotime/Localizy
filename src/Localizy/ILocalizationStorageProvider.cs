using System.Collections.Generic;
using System.Globalization;

namespace Localizy
{
    public interface ILocalizationStorageProvider
    {
        string Name { get; }
        IEnumerable<LocalString> Provide(CultureInfo culture);
    }
}