using System.Globalization;

namespace Localizy
{
    public interface ICurrentCultureContext
    {
        CultureInfo CurrentCulture { get; set; }
        CultureInfo CurrentUICulture { get; set; }
    }
}