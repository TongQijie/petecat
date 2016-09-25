using System.Xml;
using System.Xml.Serialization;

namespace Petecat.IoC.Configuration
{
    public class ContainerObjectArgumentConfig : ContainerObjectValueElementConfig
    {
        public ContainerObjectArgumentConfig()
        {
            Index = -1;
        }

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("index")]
        public int Index { get; set; }
    }
}
