using System.Globalization;

namespace Localizy
{
    public class LocalizationManagerAdapter : ILocalizationManager
    {
        private ILocalizationProvider _localizationProvider;

        public LocalizationManagerAdapter(ILocalizationProvider localizationProvider)
        {
            _localizationProvider = localizationProvider;
        }

        public string GetText(StringToken stringToken, CultureInfo culture = null)
        {
            return _localizationProvider.GetText(stringToken, culture);
        }
    }
}