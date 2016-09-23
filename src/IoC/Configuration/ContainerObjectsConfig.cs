using System.Xml.Serialization;

namespace Petecat.IoC.Configuration
{
    [XmlRoot("objects")]
    public class ContainerObjectsConfig
    {
        [XmlElement("assembly")]
        public ContainerAssemblyConfig[] Assemblies { get; set; }

        [XmlElement("import")]
        public ContainerImporterConfig[] Importers { get; set; }

        [XmlElement("object")]
        public ContainerObjectConfig[] Objects { get; set; }
    }
}
