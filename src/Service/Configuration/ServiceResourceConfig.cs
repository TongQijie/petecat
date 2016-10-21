using System.Xml.Serialization;

namespace Petecat.Service.Configuration
{
    public class ServiceResourceConfig
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("method")]
        public string Method { get; set; }

        [XmlAttribute("url")]
        public string Url { get; set; }

        [XmlAttribute("host")]
        public string Host { get; set; }

        [XmlElement("contentType")]
        public string ContentType { get; set; }

        [XmlElement("accept")]
        public string Accept { get; set; }

        [XmlAttribute("port")]
        public int Port { get; set; }

        [XmlAttribute("serviceName")]
        public string ServiceName { get; set; }

        [XmlAttribute("methodName")]
        public string MethodName { get; set; }
    }
}
