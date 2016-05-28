using Petecat.Data.Ini;
using System.Xml.Serialization;

namespace Petecat.Test.Configuration
{
    [XmlRoot("appSettings")]
    public class AppSettings
    {
        [IniKey("enableHttps")]
        [XmlElement("enableHttps")]
        public bool EnableHttps { get; set; }

        [XmlElement("httpsConfig")]
        public HttpsConfig HttpsConfig { get; set; }
    }

    public class HttpsConfig
    {
        [IniKey("url")]
        [XmlAttribute("url")]
        public string Url { get; set; }

        [IniKey("port")]
        [XmlAttribute("port")]
        public int Port { get; set; }
    }
}
