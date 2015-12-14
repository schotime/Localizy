namespace Localizy.Tests
{
    public class TestTranslations : StringToken
    {
        public static StringToken TestTop = new TestTranslations("TestTop");

        public class General
        {
            public static StringToken Test1 = new StringToken<General>("Test1");
            public static StringToken Test1Missing = new StringToken<General>("Test1Missing");
        }

        protected TestTranslations(string defaultValue) : base(null, defaultValue)
        {
        }
    }
}