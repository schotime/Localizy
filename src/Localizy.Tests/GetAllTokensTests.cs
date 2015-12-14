using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Localizy.Storage;
using Xunit;

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

            _provider = new LocalizationProvider(_localizationStorageProvider)
            {
                CurrentCultureFactory = () => new CultureInfo("en")
            };
        }

        [Fact]
        public void GetAllTokens_ShouldReturnAllStringTokensForTheGivenAssemblyFilteredByAWhereClause()
        {
            var text1en = _provider.GetAllTokens(new CultureInfo("en"), typeof(TestTranslations).Assembly, x => x == typeof(TestTranslations)).ToList();
            Assert.Equal(3, text1en.Count());
            Assert.Equal("TestTop", _provider.GetText(text1en[0]));
            Assert.Equal("Test1en", _provider.GetText(text1en[1]));
            Assert.Equal("Test1Missing", _provider.GetText(text1en[2]));
        }
    }
}