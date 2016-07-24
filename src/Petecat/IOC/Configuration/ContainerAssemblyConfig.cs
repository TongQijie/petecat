using System.Xml.Serialization;

namespace Petecat.IOC.Configuration
{
    public class ContainerAssemblyConfig
    {
        [XmlAttribute("path")]
        public string Path { get; set; }
    }
}
