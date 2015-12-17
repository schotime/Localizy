using System.Globalization;

namespace Localizy
{
    public class LocalizationManagerStaticAdapter : ILocalizationManager
    {
        public string GetText(StringToken stringToken, CultureInfo culture = null)
        {
            return LocalizationManager.GetText(stringToken, culture);
        }
    }
}