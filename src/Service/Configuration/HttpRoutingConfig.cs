using System.Xml.Serialization;
namespace Petecat.Service.Configuration
{
    public class HttpRoutingConfig
    {
        [XmlElement("add")]
        public KeyValueConfig[] KeyValues { get; set; }
    }
}