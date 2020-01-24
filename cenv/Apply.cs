using System;
using System.Xml;
using Nett;

namespace Metadev.cenv
{
    public class Apply
    {
        public static int Execute(ArgsOptions options)
        {
            var config = Toml.ReadFile(options.EnvFile);
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(options.TemplateFile);

            foreach (var key in config.Keys)
            {
                var tomlBlock = config[key];
                var k = key.ToLowerInvariant();
                if (k == "connectionstrings")
                {
                    ApplyConnectionStrings(tomlBlock as TomlTable, xmlDoc);
                }
                else if (k == "appsettings")
                {
                    ApplyAppSettings(tomlBlock as TomlTable, xmlDoc);
                }
            }
            if (string.IsNullOrEmpty(options.OutputFile) || options.DryRun)
            {
                Console.WriteLine(Utils.XmlToString(xmlDoc));
            }
            else 
            {
                xmlDoc.Save(options.OutputFile);
            }
            return 0;
        }

        static void ApplyConnectionStrings(TomlTable table, XmlDocument xmlDoc)
        {
            foreach (var csKey in table.Keys)
            {
                var itemValues = table[csKey] as TomlTable;
                var connectionString = itemValues["connectionString"].ToString();
                var providerName = itemValues["providerName"].ToString();

                var xmlNode = xmlDoc.SelectSingleNode("configuration/connectionStrings/add[@name='" + csKey + "']");
                if (xmlNode != null)
                {
                    // update
                    xmlNode.Attributes["connectionString"].Value = connectionString;
                    xmlNode.Attributes["providerName"].Value = providerName;
                }
                else
                {
                    // Not existing, create element
                    var xmlRoot = xmlDoc.SelectSingleNode("configuration/connectionStrings");
                    var xmlItem = xmlDoc.CreateNode("element", "add", "");

                    XmlAttribute atr0 = xmlDoc.CreateAttribute("name");
                    atr0.Value = csKey;
                    XmlAttribute atr1 = xmlDoc.CreateAttribute("connectionString");
                    atr1.Value = connectionString;
                    XmlAttribute atr2 = xmlDoc.CreateAttribute("providerName");
                    atr2.Value = providerName;

                    xmlItem.Attributes.Append(atr0);
                    xmlItem.Attributes.Append(atr1);
                    xmlItem.Attributes.Append(atr2);
                    xmlRoot.AppendChild(xmlItem);
                }
            }
        }
        static void ApplyAppSettings(TomlTable table, XmlDocument xmlDoc)
        {
            foreach (var csKey in table.Keys)
            {
                var value = table[csKey].ToString();

                var xmlNode = xmlDoc.SelectSingleNode("configuration/appSettings/add[@key='" + csKey + "']");
                if (xmlNode != null)
                {
                    // update
                    xmlNode.Attributes["value"].Value = value;
                }
                else
                {
                    // inject
                    var xmlRoot = xmlDoc.SelectSingleNode("configuration/appSettings");
                    var xmlItem = xmlDoc.CreateNode("element", "add", "");

                    XmlAttribute atr0 = xmlDoc.CreateAttribute("key");
                    atr0.Value = csKey;
                    XmlAttribute atr1 = xmlDoc.CreateAttribute("value");
                    atr1.Value = value;

                    xmlItem.Attributes.Append(atr0);
                    xmlItem.Attributes.Append(atr1);
                    xmlRoot.AppendChild(xmlItem);
                }
            }
        }
    }
}
