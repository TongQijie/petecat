using System.Xml.Serialization;

namespace Petecat.IoC.Configuration
{
    [XmlRoot("list")]
    public class ContainerObjectValueCollectionConfig
    {
        [XmlElement("li")]
        public ContainerObjectValueElementConfig[] Elements { get; set; }
    }
}
