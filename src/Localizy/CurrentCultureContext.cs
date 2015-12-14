using System.Globalization;
using System.Threading;

namespace Localizy
{
    public class CurrentCultureContext : ICurrentCultureContext
    {
#if DOTNET54
        public CultureInfo CurrentCulture
        {
            get { return CultureInfo.CurrentCulture; }
            set { CultureInfo.CurrentCulture = value; }
        }

        public CultureInfo CurrentUICulture
        {
            get { return CultureInfo.CurrentUICulture; }
            set { CultureInfo.CurrentUICulture = value; }
        }
#else
        public CultureInfo CurrentCulture
        {
            get { return Thread.CurrentThread.CurrentCulture; }
            set { Thread.CurrentThread.CurrentCulture = value; }
        }

        public CultureInfo CurrentUICulture
        {
            get { return Thread.CurrentThread.CurrentUICulture; }
            set { Thread.CurrentThread.CurrentUICulture = value; }
        }
#endif
    }
}