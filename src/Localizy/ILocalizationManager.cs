using System.Globalization;

namespace Localizy
{
    public interface ILocalizationManager
    {
        string GetText(StringToken stringToken, CultureInfo culture = null);
    }
}