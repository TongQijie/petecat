using System.Xml.Serialization;

namespace Petecat.IOC.Configuration
{
    public class ContainerObjectConfig
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("type")]
        public string Type { get; set; }

        [XmlAttribute("singleton")]
        public bool Singleton { get; set; }

        [XmlElement("arg")]
        public ContainerArgumentConfig[] Arguments { get; set; }
    }
}