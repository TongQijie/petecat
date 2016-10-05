using System.Xml.Serialization;

namespace Petecat.Configuring.Configuration
{
    [XmlRoot("configuration")]
    public class ConfigurationItemsConfig
    {
        [XmlElement("item")]
        public ConfigurationItemConfig[] Items { get; set; }
    }
}

