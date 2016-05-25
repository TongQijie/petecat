using System.Xml.Serialization;

namespace Petecat.Test.Configuration
{
    [XmlRoot("appSettings")]
    public class AppSettings
    {
        [XmlElement("enableHttps")]
        public bool EnableHttps { get; set; }

        [XmlElement("httpsConfig")]
        public HttpsConfig HttpsConfig { get; set; }
    }

    public class HttpsConfig
    {
        [XmlAttribute("url")]
        public string Url { get; set; }

        [XmlAttribute("port")]
        public int Port { get; set; }
    }
}
