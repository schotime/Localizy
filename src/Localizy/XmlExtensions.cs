using System.IO;
using System.Xml;

namespace Localizy
{
    internal static class XmlExtensions
    {
        public static XmlDocument XmlFromFileWithRoot(this string fileName, string root)
        {
            if (File.Exists(fileName))
            {
                return new XmlDocument().FromFile(fileName);
            }

            var document = new XmlDocument();
            document.WithRoot(root);

            return document;
        }

        public static XmlDocument FromFile(this XmlDocument document, string fileName)
        {
            using (var fileStream = new FileStream(fileName, FileMode.Open))
            {
                document.Load(fileStream);
            }
            return document;
        }

        public static XmlElement WithRoot(this XmlDocument document, string elementName)
        {
            var element = document.CreateElement(elementName);
            document.AppendChild(element);

            return element;
        }
    }
}