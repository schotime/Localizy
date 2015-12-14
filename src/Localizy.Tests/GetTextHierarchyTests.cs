using System.Collections.Generic;
using System.Globalization;
using Localizy.Storage;
using Xunit;

namespace Localizy.Tests
{
    public class GetTextHierarchyTests
    {
        private readonly LocalizationProvider _provider;

        public GetTextHierarchyTests()
        {
            var storageProvider = new InMemoryLocalizationStorageProvider("1", new Dictionary<CultureInfo, List<LocalString>>()
            {
                {
                    new CultureInfo("en"), new List<LocalString>()
                    {
                        new LocalString("TestTranslations.General:Test1", "Test1en")
                    }
                },
                {
                    new CultureInfo("en-US"), new List<LocalString>()
                    {
                        new LocalString("TestTranslations.General:Test1", "Test1en-US")
                    }
                }
            });

            _provider = new LocalizationProvider(storageProvider);
        }

        [Fact]
        public void GetTextWithRegionalCultureSpecified_ShouldFallbackToParentCultureIfMissing()
        {
            var stringToken = _provider.GetText(TestTranslations.General.Test1, new CultureInfo("en-AU"));

            Assert.Equal("Test1en", stringToken);
        }
        
        [Fact]
        public void GetTextWithRegionalCultureSpecified_ShouldTranslateUsingTheRegionalCulture()
        {
            var stringToken = _provider.GetText(TestTranslations.General.Test1, new CultureInfo("en-US"));

            Assert.Equal("Test1en-US", stringToken);
        }
    }
}