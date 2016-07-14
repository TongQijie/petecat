using System.Xml.Serialization;

namespace Petecat.Service.Configuration
{
    [XmlRoot("serviceAssemblies")]
    public class ServiceAssembliesConfig
    {
        [XmlElement("assembly")]
        public ServiceAssemblyConfig[] ServiceAssemblies { get; set; }
    }
}