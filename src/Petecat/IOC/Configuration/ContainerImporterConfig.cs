using System.Xml.Serialization;

namespace Petecat.IOC.Configuration
{
    public class ContainerImporterConfig
    {
        [XmlAttribute("path")]
        public string Path { get; set; }
    }
}
