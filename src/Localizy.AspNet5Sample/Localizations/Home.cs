using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Localizy;

namespace Localizy.AspNet5Sample.Localizations
{
    public partial class L
    {
        public class Home
        {
            public static StringToken APPLICATION_USES = new StringToken<Home>("Application Uses");
            public static StringToken SAMPLE_PAGES = new StringToken<Home>("Sample pages using ASP.NET MVC 6");
        }
    }
}
