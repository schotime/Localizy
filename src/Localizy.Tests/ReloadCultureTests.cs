using System.Collections.Generic;
using System.Globalization;
using Localizy.Storage;
using Xunit;
using Xunit.Abstractions;
using System;
using System.Reflection;

namespace Localizy.Tests
{
    public class ReloadCultureTests
    {
        private readonly LocalizationProvider _provider;
        private InMemoryLocalizationStorageProvider _localizationStorageStorageProvider;
        private Dictionary<CultureInfo, List<LocalString>> _data;

        public ReloadCultureTests()
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

            _localizationStorageStorageProvider = new InMemoryLocalizationStorageProvider("1", _data);

            _provider = new LocalizationProvider(typeof(TestTranslations).GetTypeInfo().Assembly, _localizationStorageStorageProvider)
            {
                CurrentCultureFactory = () => new CultureInfo("en")
            };
        }

        [Fact]
        public void ReloadingASpecificCulture_ShouldOnlyReloadThatCultureAndNotAllCultures()
        {
            //Prime cache
            _provider.TryGetText("TestTranslations.General:Test1", new CultureInfo("en"));
            _provider.TryGetText("TestTranslations.General:Test1", new CultureInfo("fr"));
            
            //Replace source
            _data[new CultureInfo("en")] = new List<LocalString>()
            {
                new LocalString("TestTranslations.General:Test1", "Test1en-reload")
            };

            _data[new CultureInfo("fr")] = new List<LocalString>()
            {
                new LocalString("TestTranslations.General:Test1", "Test1fr-reload")
            };

            //Reload en cache
            _provider.Reload(new CultureInfo("en"));

            var text2en = _provider.TryGetText("TestTranslations.General:Test1", new CultureInfo("en"));
            var text2fr = _provider.TryGetText("TestTranslations.General:Test1", new CultureInfo("fr"));
            Assert.Equal("Test1en-reload", text2en);
            Assert.Equal("Test1fr", text2fr);
        }
    }
}