
using System.Xml;
using System;
using Nett;

namespace Metadev.cenv
{
    public class Check
    {
        static int Errors = 0;
        public static bool IsMissing(string value, ArgsOptions options)
        {
            return value == options.ToBeDefinedKeyword;
        }
        public static void ReportConnectionStringError(string section, string key, string property, string currentValue)
        {
            Console.WriteLine(string.Format("{0,3} Error U1: {1}/{2} @{3,-20} Is not defined. Actual='{4}'",
                ++Errors,
                section,
                key,
                property,
                currentValue));
        }
        public static void ReportAppSettingError(string section, string key, string currentValue)
        {
            Console.WriteLine(string.Format("{0,3} Error U2: {1}/{2,-32} Is not defined. Actual='{3}'",
                ++Errors, 
                section,
                key,
                currentValue));
        }
        public static void ReportConnectionStringMismatch(string section, string key, string property, string configValue, string actualValue)
        {
            Console.WriteLine(string.Format("{0,3} Error M1: {1}/{2} @{3,-20} Does not match. Config: '{4}' Actual: '{5}'",
                ++Errors,
                section,
                key,
                property,
                configValue,
                actualValue));
        }
        public static void ReportAppSettingMismatch(string section, string key, string configValue, string actualValue)
        {
            Console.WriteLine(string.Format("{0,3} Error M2: {1}/{2,-32} Does not match. Config: '{3}' Actual: '{4}'",
                ++Errors,
                section,
                key,
                configValue,
                actualValue));
        }
        public static int Execute(ArgsOptions options)
        {
            var config = Toml.ReadFile(options.EnvFile);
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(options.TemplateFile);

            var xmlNodes = xmlDoc.SelectNodes("configuration/connectionStrings/add");
            var tomlCS = config["connectionStrings"] as TomlTable;

            foreach (XmlElement xmlNode in xmlNodes)
            {
                var name = xmlNode.GetAttribute("name");
                var connectionString = xmlNode.GetAttribute("connectionString");
                var providerName = xmlNode.GetAttribute("providerName");

                if (IsMissing(connectionString, options))
                {
                    ReportConnectionStringError("connectionStrings", name, "connectionString", connectionString);
                }
                if (IsMissing(providerName, options))
                {
                    ReportConnectionStringError("connectionStrings", name, "providerName", providerName);
                }

                if (tomlCS != null)
                {
                    var tomlSection = tomlCS[name] as TomlTable;
                    var tomlConnectionString = (tomlSection as TomlTable)["connectionString"].ToString();
                    var tomlProviderName = (tomlSection as TomlTable)["providerName"].ToString();

                    if (tomlConnectionString != connectionString)
                    {
                        ReportConnectionStringMismatch("connectionStrings", name, "connectionString", tomlConnectionString, connectionString);
                    }
                    if (tomlProviderName != providerName)
                    {
                        ReportConnectionStringMismatch("connectionStrings", name, "connectionString", tomlProviderName, providerName);
                    }
                }
            }
            var xmlNodes2 = xmlDoc.SelectNodes("configuration/appSettings/add");
            var tomlAs = config["appSettings"] as TomlTable;

            foreach (XmlElement xmlNode in xmlNodes2)
            {
                var key = xmlNode.GetAttribute("key");
                var value = xmlNode.GetAttribute("value");

                if (IsMissing(value, options))
                {
                    ReportAppSettingError("appSettings", key, value);
                }
                if (tomlAs != null)
                {
                    var tomlObj = tomlAs.TryGetValue(key);
                    var tomlValue = tomlObj != null ? tomlObj.ToString() : null;

                    if (tomlValue != value)
                    {
                        ReportAppSettingMismatch("appSettings", key, tomlValue, value);
                    }
                }
            }

            if (Errors == 0)
            {
                return 0;
            }
            else
            {
                Console.WriteLine(string.Format("{0} errors were found.", Errors));
                return 42;
            }
        }
    }
}
