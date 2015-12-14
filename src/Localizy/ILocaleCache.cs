using System;
using System.Collections.Generic;
using System.Globalization;

namespace Localizy
{
    public interface ILocaleCache
    {
        void Update(LocalizationKey key, string value);
        string Get(LocalizationKey key, Func<string> missing);
        CultureInfo Culture { get; }
    }
}