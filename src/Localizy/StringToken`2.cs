using System;
using System.Globalization;

namespace Localizy
{
    public class StringToken<T, TParams> : StringToken<T>, IStringTokenWithParams
    {
        public StringToken(string defaultValue): base (defaultValue)
        {
        }

        public string FormatTokenWith(TParams tokenModel)
        {
            return FormatTokenWith(tokenModel, null);
        }

        public string FormatTokenWith(TParams tokenModel, CultureInfo culture)
        {
            return ObjectFormatter.TokenFormat(base.ToString(culture), tokenModel);
        }

        public string ToRawString()
        {
            return base.ToString();
        }

        public override string ToString()
        {
            throw new Exception("You must not call ToString() directly. Use either FormatTokenWith(tokenModel) or ToRawString()");
        }
    }
}