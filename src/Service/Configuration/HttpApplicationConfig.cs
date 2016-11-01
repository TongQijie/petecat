using System.Xml.Serialization;
namespace Petecat.Service.Configuration
{
    [XmlRoot("application")]
    public class HttpApplicationConfig
    {
        [XmlElement("staticResourceContentMapping")]
        public StaticResourceContentMapping StaticResourceContentMapping { get; set; }

        [XmlElement("httpRouting")]
        public HttpRoutingConfig HttpRoutings { get; set; }
    }
}