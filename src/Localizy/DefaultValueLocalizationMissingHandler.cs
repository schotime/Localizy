using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace Localizy
{
    public class DefaultValueLocalizationMissingHandler : ILocalizationMissingHandler
    {
        public string FindMissingText(StringToken key, CultureInfo culture)
        {
            return key.DefaultValue;
        }
    }
}