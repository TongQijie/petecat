using System.Xml.Serialization;

namespace Petecat.IOC.Configuration
{
    [XmlRoot("assemblies")]
    public class ContainerAssembliesConfig
    {
        [XmlElement("assembly")]
        public ContainerAssemblyConfig[] Assemblies { get; set; }
    }
}