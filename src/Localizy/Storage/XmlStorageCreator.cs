using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Localizy.Storage
{
    public class XmlStorageGenerator
    {
        private readonly ILocalizationProvider _localizationProvider;

        public XmlStorageGenerator(ILocalizationProvider localizationProvider)
        {
            _localizationProvider = localizationProvider;
        }

        public void GenerateFile(string directory, Assembly assembly, Func<Type, bool> filter, CultureInfo culture, bool generateHash = false, bool useDefault = true)
        {
            var result = Generate(assembly, filter, culture, generateHash: generateHash, useDefault: useDefault);
            var file = Path.Combine(directory, culture.Name + XmlDirectoryStorageProvider.DefaultSuffix);
            File.WriteAllText(file, result);
        }

        public string Generate(Assembly assembly, Func<Type, bool> filter, CultureInfo culture, bool generateHash = false, bool useDefault = true)
        {
            var tokens = _localizationProvider.GetAllTokens(culture, assembly, filter);
            var result = GenerateXml(tokens.Select(x => new LocalString(x.ToLocalizationKey().ToString(), useDefault ? x.DefaultValue : _localizationProvider.GetText(x, culture))), generateHash);
            return result;
        }

        public static string GenerateXml(IEnumerable<LocalString> strings, bool generateHash)
        {
            var document = new XmlDocument();
            var root = document.WithRoot(XmlDirectoryStorageProvider.RootElement);

            strings.OrderBy(x => x.Display).Each(
                x =>
                {
                    var el = root.OwnerDocument.CreateElement(XmlDirectoryStorageProvider.LeafElement);
                    root.AppendChild(el);

                    el.SetAttribute(XmlDirectoryStorageProvider.Key, x.Key);
                    if (generateHash)
                    {
                        el.SetAttribute(XmlDirectoryStorageProvider.Hash, CreateMD5(x.Display));
                    }
                    el.InnerText = x.Display;
                });

            return document.OuterXml;
        }

        private static string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            MD5 md5 = MD5.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            // Convert the byte array to hexadecimal string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("X2"));
            }
            return sb.ToString();
        }
    }
}
