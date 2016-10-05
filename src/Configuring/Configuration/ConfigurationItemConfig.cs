using System.Xml.Serialization;

namespace Petecat.Configuring.Configuration
{
    public class ConfigurationItemConfig
    {
        [XmlAttribute("key")]
        public string Key { get; set; }

        [XmlAttribute("path")]
        public string Path { get; set; }

        [XmlAttribute("type")]
        public string Type { get; set; }
    }
}

