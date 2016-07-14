using System.Xml.Serialization;

namespace Petecat.Service.Configuration
{
    public class ServiceAssemblyConfig
    {
        [XmlAttribute("path")]
        public string Path { get; set; }
    }
}
