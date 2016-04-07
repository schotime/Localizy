using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml;

namespace Localizy.Storage
{
    public class XmlDirectoryStorageProvider : ILocalizationStorageProvider
    {
        private DirectoryInfo _directoryInfo;
        public const string RootElement = "localizations";
        public const string Key = "key";
        public const string LeafElement = "string";
        public const string DefaultSuffix = ".locale.config";
        public const string Hash = "hash";

        public XmlDirectoryStorageProvider(string name, string directory, string fileSuffixPattern = "*" + DefaultSuffix)
        {
            Name = name;
            FileSuffixPattern = fileSuffixPattern;

            _directoryInfo = new DirectoryInfo(directory);
            
            if (!_directoryInfo.Exists)
            {
                throw new ArgumentException("Directory doesn't exist", "directory");
            }
        }

        public string Name { get; protected set; }
        public string FileSuffixPattern { get; protected set; }

        public virtual IEnumerable<LocalString> Provide(CultureInfo culture)
        {
            var file = _directoryInfo.EnumerateFiles(FileSuffixPattern).FirstOrDefault(x => !string.IsNullOrEmpty(culture.Name) && x.Name.Split('.').First() == culture.Name);
            if (file == null)
                return Enumerable.Empty<LocalString>();

            return LoadFrom(file.FullName);
        }

        public virtual IEnumerable<LocalString> LoadFrom(string file)
        {
            var document = file.XmlFromFileWithRoot(RootElement);

            foreach (XmlElement element in document.DocumentElement.SelectNodes(LeafElement))
            {
                yield return ToLocalString(element);
            }
        }

        protected static LocalString ToLocalString(XmlElement element)
        {
            return new LocalString(element.GetAttribute(Key), element.InnerText);
        }
    }
}