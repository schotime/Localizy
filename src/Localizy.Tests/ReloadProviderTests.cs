using System;
using System.Collections.Generic;
using System.Globalization;
using Localizy.Storage;
using Xunit;
using System.Reflection;

namespace Localizy.Tests
{
    public class ReloadProviderTests
    {
        private readonly LocalizationProvider _provider;
        private InMemoryLocalizationStorageProvider _localizationStorageStorageProvider1;
        private InMemoryLocalizationStorageProvider _localizationStorageStorageProvider2;
        private Dictionary<CultureInfo, List<LocalString>> _data;
        private Dictionary<CultureInfo, List<LocalString>> _data2;

        public ReloadProviderTests()
        {
            _data = new Dictionary<CultureInfo, List<LocalString>>()
            {
                {
                    new CultureInfo("en"), new List<LocalString>()
                    {
                        new LocalString("TestTranslations.General:Test1", "Test1en"),
                        new LocalString("TestTranslations.General:Test1Missing", "Test2en")
                    }
                },
                {
                    new CultureInfo("fr"), new List<LocalString>()
                    {
                        new LocalString("TestTranslations.General:Test1", "Test1fr")
                    }
                }
            };

            _data2 = new Dictionary<CultureInfo, List<LocalString>>()
            {
                {
                    new CultureInfo("en"), new List<LocalString>()
                    {
                        new LocalString("TestTranslations.General:Test1", "Test1en2")
                    }
                },
                {
                    new CultureInfo("fr"), new List<LocalString>()
                    {
                        new LocalString("TestTranslations.General:Test1", "Test1fr2")
                    }
                }
            };

            _localizationStorageStorageProvider1 = GetLocalizationStorageStorageProvider1();
            _localizationStorageStorageProvider2 = GetLocalizationStorageStorageProvider2();

            _provider = new LocalizationProvider(typeof(TestTranslations).GetTypeInfo().Assembly, _localizationStorageStorageProvider1, _localizationStorageStorageProvider2)
            {
                CurrentCultureFactory = () => new CultureInfo("en")
            };
        }

        private InMemoryLocalizationStorageProvider GetLocalizationStorageStorageProvider1()
        {
            return new InMemoryLocalizationStorageProvider("1", _data);
        }

        private InMemoryLocalizationStorageProvider GetLocalizationStorageStorageProvider2()
        {
            return new InMemoryLocalizationStorageProvider("2", _data2);
        }

        [Fact]
        public void ReloadingProvider_ShouldReloadTheCacheForJustThatProvider()
        {
            //Prime cache
            _provider.TryGetText("TestTranslations.General:Test1", new CultureInfo("en"));
            _provider.TryGetText("TestTranslations.General:Test1", new CultureInfo("fr"));

            //Replace source
            _data[new CultureInfo("en")] = new List<LocalString>()
            {
                new LocalString("TestTranslations.General:Test1Missing", "Test2en-reload")
            };

            _data2[new CultureInfo("en")] = new List<LocalString>()
            {
                new LocalString("TestTranslations.General:Test1", "Test1en-reload")
            };

            _data2[new CultureInfo("fr")] = new List<LocalString>()
            {
                new LocalString("TestTranslations.General:Test1", "Test1fr-reload")
            };

            //Reload cache 2
            _provider.Reload("2");

            var text2en = _provider.TryGetText("TestTranslations.General:Test1", new CultureInfo("en"));
            var text2fr = _provider.TryGetText("TestTranslations.General:Test1", new CultureInfo("fr"));
            var text2en2 = _provider.TryGetText("TestTranslations.General:Test1Missing", new CultureInfo("en"));
            Assert.Equal("Test1en-reload", text2en);
            Assert.Equal("Test1fr-reload", text2fr);
            Assert.Equal("Test2en", text2en2);
        }

        [Fact]
        public void ReloadingProvider_ShouldReloadTheCacheForJustThatProvider2()
        {
            //Prime cache
            _provider.TryGetText("TestTranslations.General:Test1", new CultureInfo("en"));
            _provider.TryGetText("TestTranslations.General:Test1", new CultureInfo("fr"));

            //Replace source
            _data[new CultureInfo("en")] = new List<LocalString>()
            {
                new LocalString("TestTranslations.General:Test1Missing", "Test2en-reload")
            };

            _data2[new CultureInfo("en")] = new List<LocalString>()
            {
                new LocalString("TestTranslations.General:Test1", "Test1en-reload")
            };

            _data2[new CultureInfo("fr")] = new List<LocalString>()
            {
                new LocalString("TestTranslations.General:Test1", "Test1fr-reload")
            };

            //Reload cache 1
            _provider.Reload("1");

            var text2en = _provider.TryGetText("TestTranslations.General:Test1", new CultureInfo("en"));
            var text2fr = _provider.TryGetText("TestTranslations.General:Test1", new CultureInfo("fr"));
            var text2en2 = _provider.TryGetText("TestTranslations.General:Test1Missing", new CultureInfo("en"));
            Assert.Equal("Test1en2", text2en);
            Assert.Equal("Test1fr2", text2fr);
            Assert.Equal("Test2en-reload", text2en2);
        }
    }
}