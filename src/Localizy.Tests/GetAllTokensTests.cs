using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Localizy.Storage;
using Xunit;
using System.Reflection;

namespace Localizy.Tests
{
    public class GetAllTokensTests
    {
        private readonly LocalizationProvider _provider;
        private InMemoryLocalizationStorageProvider _localizationStorageProvider;
        private Dictionary<CultureInfo, List<LocalString>> _data;

        public GetAllTokensTests()
        {
            _data = new Dictionary<CultureInfo, List<LocalString>>()
            {
                {
                    new CultureInfo("en"), new List<LocalString>()
                    {
                        new LocalString("TestTranslations.General:Test1", "Test1en")
                    }
                },
                {
                    new CultureInfo("fr"), new List<LocalString>()
                    {
                        new LocalString("TestTranslations.General:Test1", "Test1fr")
                    }
                }
            };

            _localizationStorageProvider = new InMemoryLocalizationStorageProvider("1", _data);

            _provider = new LocalizationProvider(typeof(TestTranslations).GetTypeInfo().Assembly, _localizationStorageProvider).WithFilter(x => x == typeof(TestTranslations));
        }

        [Fact]
        public void GetAllTokens_ShouldReturnAllStringTokensForTheGivenAssemblyFilteredByAWhereClause()
        {
            var text1en = _provider.GetAllTokens();
            Assert.Equal(3, text1en.Count());
            Assert.Equal("TestTop", _provider.GetText(text1en["TestTranslations.TestTop"], new CultureInfo("en")));
            Assert.Equal("Test1en", _provider.GetText(text1en["TestTranslations.General:Test1"], new CultureInfo("en")));
            Assert.Equal("Test1Missing", _provider.GetText(text1en["TestTranslations.General:Test1Missing"], new CultureInfo("en")));
        }
    }
}