using System.Globalization;

namespace Localizy
{
    public interface ILocalizationMissingHandler
    {
        string FindMissingText(StringToken key, CultureInfo culture);
    }
}