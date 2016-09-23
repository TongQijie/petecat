using System.Xml.Serialization;

namespace Petecat.Service.Configuration
{
    public class ServiceHostConfig
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("url")]
        public string Url { get; set; }
    }
}
