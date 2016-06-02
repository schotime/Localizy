using System.Collections.Generic;
using System.Globalization;
using Localizy.Storage;
using Xunit;
using System.Reflection;

namespace Localizy.Tests
{
    public class GetTextTests
    {
        private readonly LocalizationProvider _provider;

        public GetTextTests()
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

            _provider = new LocalizationProvider(typeof(TestTranslations).GetTypeInfo().Assembly, storageProvider)
            {
                CurrentCultureFactory = () => new CultureInfo("en")
            };
        }

        [Fact]
        public void GetTextWithNoCultureSpecified_ShouldTranslateUsingTheCurrentCultureFactory()
        {
            var stringToken = _provider.GetText(TestTranslations.General.Test1);

            Assert.Equal("Test1en", stringToken);
        }

        [Fact]
        public void GetTextWithCultureSpecified_ShouldTranslateUsingTheCultureSpecified()
        {
            var stringToken = TestTranslations.General.Test1;

            var result = _provider.GetText(stringToken, new CultureInfo("fr"));

            Assert.Equal("Test1fr", result);
        }

        [Fact]
        public void GetTextWithNoTranslationFound_ShouldReturnDefaultValue()
        {
            var stringToken = TestTranslations.General.Test1Missing;
            var result = _provider.GetText(stringToken, new CultureInfo("fr"));

            Assert.Equal("Test1Missing", result);
        }

        [Fact]
        public void MultipleCallsToGetText_ShouldReturnTheSameResult()
        {
            var stringToken1 = _provider.GetText(TestTranslations.General.Test1);
            var stringToken2 = _provider.GetText(TestTranslations.General.Test1);

            Assert.Equal(stringToken1, stringToken2);
        }

        [Fact]
        public void GetTextUsingTheKeyAndSpecifyingTheCulture_ShouldTranslateTheSameKeyIntoBothCultures()
        {
            var stringToken1 = "TestTranslations.General:Test1";
            var result1 = _provider.TryGetText(stringToken1, new CultureInfo("fr"));
            var result2 = _provider.TryGetText(stringToken1, new CultureInfo("en"));

            Assert.Equal("Test1fr", result1);
            Assert.Equal("Test1en", result2);
        }

        [Fact]
        public void GetTextUsingTheKeyAndSpecifyingNoCulture_ShouldTranslateUsingTheCurrentCultureFactory()
        {
            var stringToken1 = _provider.TryGetText("TestTranslations.General:Test1");
            Assert.Equal("Test1en", stringToken1);
        }

        [Fact]
        public void GetTextUsingTheKeyWithDotAndSpecifyingNoCulture_ShouldTranslateUsingTheCurrentCultureFactory()
        {
            var stringToken1 = _provider.TryGetText("TestTranslations.General.Test1");
            Assert.Equal("Test1en", stringToken1);
        }

        [Fact]
        public void GetTextUsingTheKeyThatDoesNotExist_ShouldReturnNull()
        {
            var stringToken1 = _provider.TryGetText("MyMissingTran");
            Assert.Null(stringToken1);
        }
    }
}