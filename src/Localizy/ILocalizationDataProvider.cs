using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

namespace Localizy
{
    public interface ILocalizationDataProvider
    {
        TextAndCulture GetTextWithCulture(StringToken key, CultureInfo culture);
        void UpdateText(LocalizationKey key, CultureInfo culture, string value);
        void Reload(CultureInfo culture, Func<CultureInfo, ILocaleCache> factory);
        void Reload();
    }

    public class TextAndCulture
    {
        public TextAndCulture(string text, CultureInfo culture)
        {
            Text = text;
            Culture = culture;
        }

        public string Text { get; set; }
        public CultureInfo Culture { get; set; }
    }
}