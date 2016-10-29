using System.Xml.Serialization;
namespace Petecat.Service.Configuration
{
    [XmlRoot("application")]
    public class ServiceHttpApplicationConfig
    {
        [XmlElement("staticResourceContentMapping")]
        public StaticResourceContentMapping StaticResourceContentMapping { get; set; }
    }
}