using System.Xml.Serialization;

namespace Petecat.IOC.Configuration
{
    [XmlRoot("objects")]
    public class ContainerInstanceConfig
    {
        [XmlElement("object")]
        public ContainerObjectConfig[] Objects { get; set; }
    }
}
