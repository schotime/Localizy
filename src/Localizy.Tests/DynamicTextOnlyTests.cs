using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using Localizy.Storage;
using Xunit;

namespace Localizy.Tests
{
    public class DynamicTextOnlyTests
    {
        private readonly LocalizationProvider _provider;

        public DynamicTextOnlyTests()
        {
            var storageProvider = new InMemoryLocalizationStorageProvider("1", new Dictionary<CultureInfo, List<LocalString>>()
            {
                {
                    new CultureInfo("fr"), new List<LocalString>()
                    {
                        new LocalString("Dynamic.1", "DynamicFr")
                    }
                },
                {
                    new CultureInfo("en"), new List<LocalString>()
                    {
                        new LocalString("Dynamic.1", "DynamicEn")
                    }
                }
            });

            _provider = new LocalizationProvider(typeof(TestTranslations).GetTypeInfo().Assembly, storageProvider)
            {
                CurrentCultureFactory = () => new CultureInfo("en")
            };
        }

        [Fact]
        public void GetTextWithNoCultureSpecified_ShouldTranslateUsingTheCurrentCultureFactory()
        {
            var stringToken = _provider.GetText(StringToken.FromKeyString("Dynamic.1"), new CultureInfo("fr"));

            Assert.Equal("DynamicFr", stringToken);
        }

        [Fact]
        public void GetTextWithNoCultureSpecified_ShouldTranslateUsingTheCurrentCultureFactory1()
        {
            var stringToken = _provider.GetText(StringToken.FromKeyString("Dynamic.1"), new CultureInfo("de"));

            Assert.Null(stringToken);
        }
    }
}