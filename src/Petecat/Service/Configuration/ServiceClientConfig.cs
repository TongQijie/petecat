using System.Xml.Serialization;

namespace Petecat.Service.Configuration
{
    [XmlRoot("services")]
    public class ServiceClientConfig
    {
        [XmlArray("hosts")]
        [XmlArrayItem("host")]
        public ServiceHostConfig[] Hosts { get; set; }

        [XmlArray("resources")]
        [XmlArrayItem("resource")]
        public ServiceResourceConfig[] Resources { get; set; }
    }
}
