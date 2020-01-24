using System.IO;
using System.Xml;

namespace Metadev.cenv
{
    public class Utils
    {
        public static string XmlToString(XmlDocument xmlDoc)
        {
            var settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "  ";

            using (var stringWriter = new StringWriter())
            using (var xmlTextWriter = XmlWriter.Create(stringWriter, settings))
            {
                xmlDoc.WriteTo(xmlTextWriter);
                xmlTextWriter.Flush();
                var xmlStr = stringWriter.GetStringBuilder().ToString();
                return xmlStr;
            }
        }
    }
}
