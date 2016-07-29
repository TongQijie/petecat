using System.Xml.Serialization;

namespace Petecat.IoC.Configuration
{
    public class ContainerAssemblyConfig
    {
        [XmlAttribute("path")]
        public string Path { get; set; }
    }
}
