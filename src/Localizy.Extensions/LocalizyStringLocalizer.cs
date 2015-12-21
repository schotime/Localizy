using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;

namespace Localizy.Extensions
{
    public class LocalizyStringLocalizer<T> : IStringLocalizer<T>
    {
        private readonly ILocalizationProvider _localizationProvider;
        private readonly CultureInfo _cultureInfo;

        public LocalizyStringLocalizer(ILocalizationProvider localizationProvider)
        {
            _localizationProvider = localizationProvider;
        }

        private LocalizyStringLocalizer(ILocalizationProvider localizationProvider, CultureInfo cultureInfo)
        {
            _localizationProvider = localizationProvider;
            _cultureInfo = cultureInfo;
        }

        public IEnumerable<LocalizedString> GetAllStrings(bool includeAncestorCultures)
        {
            return _localizationProvider.GetAllTokens().Select(x => new LocalizedString(x.Value.ToLocalizationKey().ToString(), x.Value.ToString(_cultureInfo)));
        }

        LocalizedString IStringLocalizer.this[string name]
        {
            get
            {
                StringToken st;
                var tokenFound = _localizationProvider.GetAllTokens().TryGetValue(name, out st);
                var text = tokenFound ? _localizationProvider.GetText(st, _cultureInfo) : name;
                return new LocalizedString(name, text, tokenFound);
            }
        }

        LocalizedString IStringLocalizer.this[string name, params object[] arguments]
        {
            get
            {
                StringToken st;
                var tokenFound = _localizationProvider.GetAllTokens().TryGetValue(name, out st);
                var text = tokenFound ? _localizationProvider.GetText(st, _cultureInfo) : name;
                return new LocalizedString(name, string.Format(text, arguments), tokenFound);
            }
        }

        public IStringLocalizer WithCulture(CultureInfo culture)
        {
            return new LocalizyStringLocalizer<T>(_localizationProvider, culture);
        }
    }
}
