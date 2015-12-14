using System;
using System.Globalization;

namespace Localizy
{
    public class LocalizationManagerAdapter : ILocalizationManager
    {
        public string GetText(StringToken stringToken, CultureInfo culture = null)
        {
            return LocalizationManager.GetText(stringToken, LocalizationManager.GetCulture(culture));
        }
    }
}