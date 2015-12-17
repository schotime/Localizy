using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Localizy.Storage;
using Xunit;
using System;

namespace Localizy.Tests
{
    public class XmlStorageGeneratorTests
    {
        [Fact]
        public void xmlCreatorGenerate()
        {
            var provider = new LocalizationProvider(typeof(TestTranslations).Assembly).WithFilter(x => x == typeof(TestTranslations));
            var xmlCreator = new XmlStorageGenerator(provider);
            var result = xmlCreator.Generate(new CultureInfo("en"));
            Assert.Equal("<localizations><string key=\"TestTranslations.General:Test1\">Test1</string><string key=\"TestTranslations.General:Test1Missing\">Test1Missing</string><string key=\"TestTranslations:TestTop\">TestTop</string></localizations>", result);
        }

        [Fact]
        public void xmlCreatorGenerateWithHash()
        {
            var provider = new LocalizationProvider(typeof(TestTranslations).Assembly).WithFilter(x => x == typeof(TestTranslations));
            var xmlCreator = new XmlStorageGenerator(provider);
            var result = xmlCreator.Generate(new CultureInfo("en"), generateHash: true);
            Assert.Equal("<localizations><string key=\"TestTranslations.General:Test1\" hash=\"E1B849F9631FFC1829B2E31402373E3C\">Test1</string><string key=\"TestTranslations.General:Test1Missing\" hash=\"9FCA46A0D93E9890B44E801486DCB8E8\">Test1Missing</string><string key=\"TestTranslations:TestTop\" hash=\"9915829FAD74578D35CBA1767D205C3A\">TestTop</string></localizations>", result);
        }
    }
}