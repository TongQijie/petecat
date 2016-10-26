using System.Xml.Serialization;

namespace Petecat.Service.Configuration
{
    [XmlRoot("routing")]
    public class ServiceRoutingConfig
    {
        [XmlArray("rules")]
        [XmlArrayItem("rule")]
        public ServiceRoutingRuleConfig[] Rules { get; set; } 
    }
}