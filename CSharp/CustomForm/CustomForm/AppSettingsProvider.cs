using System;
using System.Collections;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;

namespace CustomForm
{
    /// <summary>
    /// Provides portable, persistent application settings.
    /// </summary>
    public class AppSettingsProvider : SettingsProvider, IApplicationSettingsProvider
    {

        private static XDocument GetXmlDoc()
        {
            // to deal with multiple settings providers accessing the same file, reload on every set or get request.
            XDocument xmlDoc = null;
            var initNew = false;
            if (File.Exists(ApplicationSettingsFile))
            {
                try
                {
                    xmlDoc = XDocument.Load(ApplicationSettingsFile);
                }
                catch { initNew = true; }
            }
            else
                initNew = true;
            if (initNew)
            {
                xmlDoc = new XDocument(new XElement("configuration", new XElement("userSettings", new XElement("Roaming"))));
            }
            return xmlDoc;
        }

        private static string ApplicationSettingsFile => Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location) ?? throw new InvalidOperationException(), "app.config");

        public override string ApplicationName { get => Assembly.GetExecutingAssembly().GetName().Name; set { } }

        public override string Name => "AppSettingsProvider";

        public override void Initialize(string name, NameValueCollection config)
        {
            if (string.IsNullOrEmpty(name)) name = "AppSettingsProvider";
            base.Initialize(name, config);
        }

        /// <summary>
        /// Applies this settings provider to each property of the given settings.
        /// </summary>
        /// <param name="settingsList">An array of settings.</param>
        public static void ApplyProvider(params ApplicationSettingsBase[] settingsList)
        {
            foreach (var settings in settingsList)
            {
                var provider = new AppSettingsProvider();
                settings.Providers.Add(provider);
                foreach (SettingsProperty prop in settings.Properties)
                    prop.Provider = provider;
                settings.Reload();
            }
        }

        public SettingsPropertyValue GetPreviousVersion(SettingsContext context, SettingsProperty property)
        {
            throw new NotImplementedException();
        }

        public void Reset(SettingsContext context)
        {
            if (File.Exists(ApplicationSettingsFile))
                File.Delete(ApplicationSettingsFile);
        }

        public void Upgrade(SettingsContext context, SettingsPropertyCollection properties)
        { /* don't do anything here*/ }

        public override SettingsPropertyValueCollection GetPropertyValues(SettingsContext context, SettingsPropertyCollection collection)
        {
            var xmlDoc = GetXmlDoc();
            var values = new SettingsPropertyValueCollection();
            // iterate through settings to be retrieved
            foreach (SettingsProperty setting in collection)
            {
                var value = new SettingsPropertyValue(setting) { IsDirty = false };
                //Set serialized value to xml element from file. This will be deserialized by SettingsPropertyValue when needed.
                var loadedValue = GetXmlValue(xmlDoc, XmlConvert.EncodeLocalName((string)context["GroupName"]), setting);
                if (loadedValue != null)
                    value.SerializedValue = loadedValue;
                else value.PropertyValue = null;
                values.Add(value);
            }
            return values;
        }

        public override void SetPropertyValues(SettingsContext context, SettingsPropertyValueCollection collection)
        {
            var xmlDoc = GetXmlDoc();
            foreach (SettingsPropertyValue value in collection)
            {
                SetXmlValue(xmlDoc, XmlConvert.EncodeLocalName((string)context["GroupName"]), value);
            }
            // Make sure that special chars such as '\r\n' are preserved by replacing them with char entities.
            using (var writer = XmlWriter.Create(ApplicationSettingsFile,
                new XmlWriterSettings { NewLineHandling = NewLineHandling.Entitize, Indent = true }))
            {
                xmlDoc.Save(writer);
            }
        }

        private static object GetXmlValue(XContainer xmlDoc, string scope, SettingsProperty prop)
        {
            object result;
            if (!IsUserScoped(prop))
                return null;
            //determine the location of the settings property
            var xmlSettings = xmlDoc.Element("configuration")?.Element("userSettings");
            xmlSettings = IsRoaming(prop) ? xmlSettings?.Element("Roaming") : xmlSettings?.Element("PC_" + Environment.MachineName);
            // retrieve the value or set to default if available
            if (xmlSettings?.Element(scope) != null && xmlSettings.Element(scope)?.Element(prop.Name) != null)
            {
                using (var reader = xmlSettings.Element(scope)?.Element(prop.Name)?.CreateReader())
                {
                    if (reader == null) return null;
                    reader.MoveToContent();
                    switch (prop.SerializeAs)
                    {
                        case SettingsSerializeAs.Xml:
                            result = reader.ReadInnerXml();
                            break;
                        case SettingsSerializeAs.Binary:
                            result = reader.ReadInnerXml();
                            result = Convert.FromBase64String(result as string);
                            break;
                        default:
                            result = reader.ReadElementContentAsString();
                            break;
                    }
                }
            }
            else
                result = prop.DefaultValue;
            return result;
        }

        private static void SetXmlValue(XContainer xmlDoc, string scope, SettingsPropertyValue value)
        {
            if (!IsUserScoped(value.Property)) return;
            //determine the location of the settings property
            var xmlSettings = xmlDoc.Element("configuration")?.Element("userSettings");
            var xmlSettingsLoc = IsRoaming(value.Property) ? xmlSettings?.Element("Roaming") : xmlSettings?.Element("PC_" + Environment.MachineName);
            // the serialized value to be saved
            XNode serialized;
            if (value.SerializedValue == null) serialized = new XText("");
            else switch (value.Property.SerializeAs)
            {
                case SettingsSerializeAs.Xml:
                    serialized = XElement.Parse((string)value.SerializedValue);
                    break;
                case SettingsSerializeAs.Binary:
                    serialized = new XText(Convert.ToBase64String((byte[])value.SerializedValue));
                    break;
                default:
                    serialized = new XText((string)value.SerializedValue);
                    break;
            }
            // check if setting already exists, otherwise create new
            if (xmlSettingsLoc == null)
            {
                xmlSettingsLoc = IsRoaming(value.Property) ? new XElement("Roaming") : new XElement("PC_" + Environment.MachineName);
                xmlSettingsLoc.Add(new XElement(scope,
                    new XElement(value.Name, serialized)));
                xmlSettings?.Add(xmlSettingsLoc);
            }
            else
            {
                var xmlScope = xmlSettingsLoc.Element(scope);
                if (xmlScope != null)
                {
                    var xmlElem = xmlScope.Element(value.Name);
                    if (xmlElem == null) xmlScope.Add(new XElement(value.Name, serialized));
                    else xmlElem.ReplaceAll(serialized);
                }
                else
                {
                    xmlSettingsLoc.Add(new XElement(scope, new XElement(value.Name, serialized)));
                }
            }
        }

        // Iterates through the properties' attributes to determine whether it's user-scoped or application-scoped.
        private static bool IsUserScoped(SettingsProperty prop)
        {
            foreach (DictionaryEntry d in prop.Attributes)
            {
                var a = (Attribute)d.Value;
                if (a is UserScopedSettingAttribute)
                    return true;
            }
            return false;
        }

        // Iterates through the properties' attributes to determine whether it's set to roam.
        private static bool IsRoaming(SettingsProperty prop)
        {
            foreach (DictionaryEntry d in prop.Attributes)
            {
                var a = (Attribute)d.Value;
                if (a is SettingsManageabilityAttribute)
                    return true;
            }
            return false;
        }
    }
}
