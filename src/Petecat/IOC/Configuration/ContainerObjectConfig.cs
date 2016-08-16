using System.Xml.Serialization;

namespace Petecat.IoC.Configuration
{
    [XmlRoot("object")]
    public class ContainerObjectConfig
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("type")]
        public string Type { get; set; }

        [XmlAttribute("singleton")]
        public bool Singleton { get; set; }

        [XmlArray("args")]
        [XmlArrayItem("arg")]
        public ContainerObjectArgumentConfig[] Arguments { get; set; }

        [XmlArray("props")]
        [XmlArrayItem("prop")]
        public ContainerObjectPropertyConfig[] Properties { get; set; }
    }
}