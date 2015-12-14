using System;

namespace Localizy
{
    public class StringToken<T> : StringToken
    {
        public StringToken(string defaultValue) : base(defaultValue)
        {
        }

        protected override Type GetContainerType()
        {
            return typeof (T);
        }
    }
}
