using System.Collections.Generic;
using System.Globalization;
using Localizy.Storage;
using Xunit;
using System.Reflection;

namespace Localizy.Tests
{
    public class GetStoredLocalizationsTests
    {
        private readonly LocalizationProvider _provider;
        private InMemoryLocalizationStorageProvider _localizationStorageProvider;
        private Dictionary<CultureInfo, List<LocalString>> _data;

        public GetStoredLocalizationsTests()
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

            _provider = new LocalizationProvider(typeof(TestTranslations).GetTypeInfo().Assembly, _localizationStorageProvider)
            {
                CurrentCultureFactory = () => new CultureInfo("en")
            };
        }

        [Fact]
        public void GetStoredLocalizations_ShouldReturnAllLocalizationForTheStorageProviderSpecified()
        {
            var storageProvider = _provider.GetStoredLocalizations("1", new CultureInfo("en"));
            Assert.Equal(1, storageProvider.Count);
            Assert.Equal(true, storageProvider.ContainsKey(new LocalizationKey("TestTranslations.General:Test1")));
        }
    }
}