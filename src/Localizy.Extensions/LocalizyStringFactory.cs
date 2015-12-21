using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;

namespace Localizy.Extensions
{
    public class LocalizyStringLocalizerFactory : IStringLocalizerFactory
    {
        private readonly ILocalizationProvider _localizationProvider;

        public LocalizyStringLocalizerFactory(ILocalizationProvider localizationProvider)
        {
            _localizationProvider = localizationProvider;
        }

        public IStringLocalizer Create(Type resourceSource)
        {
            return new LocalizyStringLocalizer<LocalString>(_localizationProvider);
        }

        public IStringLocalizer Create(string baseName, string location)
        {
            return new LocalizyStringLocalizer<LocalString>(_localizationProvider);
        }
    }

}
