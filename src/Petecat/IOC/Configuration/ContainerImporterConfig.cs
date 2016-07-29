using System.Xml.Serialization;

namespace Petecat.IoC.Configuration
{
    public class ContainerImporterConfig
    {
        [XmlAttribute("path")]
        public string Path { get; set; }
    }
}
