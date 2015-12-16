using System.Collections.Generic;
using System.Globalization;
using Localizy.Storage;
using Xunit;

namespace Localizy.Tests
{
    public class MultipleProviderTests
    {
        private readonly LocalizationProvider _provider;

        public MultipleProviderTests()
        {
            var storageProvider = new InMemoryLocalizationStorageProvider("1", new Dictionary<CultureInfo, List<LocalString>>()
            {
                {
                    new CultureInfo("fr"), new List<LocalString>()
                    {
                        new LocalString("TestTranslations.General:Test1", "Test1fr")
                    }
                },
                {
                    new CultureInfo("en"), new List<LocalString>()
                    {
                        new LocalString("TestTranslations.General:Test1", "Test1en")
                    }
                }
            });

            var storageProvider2 = new InMemoryLocalizationStorageProvider("2", new Dictionary<CultureInfo, List<LocalString>>()
            {
                {
                    new CultureInfo("fr"), new List<LocalString>()
                    {
                        new LocalString("TestTranslations.General:Test1", "Test1fr2")
                    }
                }
            });

            _provider = new LocalizationProvider(storageProvider, storageProvider2)
            {
                CurrentCultureFactory = () => new CultureInfo("en")
            };
        }

        [Fact]
        public void GetTextUsingSpecificCulture_ShouldOverlayTheProvidersInTheOrderSpecified()
        {
            var stringToken = TestTranslations.General.Test1;

            var result = _provider.GetText(stringToken, new CultureInfo("fr"));

            Assert.Equal("Test1fr2", result);
        }

        [Fact]
        public void GetTextUsingSpecificCulture_ShouldOverlayTheProvidersInTheOrderSpecifiedIfTranslationExists()
        {
            var key = "TestTranslations.General:Test1";
            var result1 = _provider.TryGetText(key, new CultureInfo("fr"));
            var result2 = _provider.TryGetText(key, new CultureInfo("en"));

            Assert.Equal("Test1fr2", result1);
            Assert.Equal("Test1en", result2);
        }
    }
}