using System.Xml.Serialization;

namespace Petecat
{
    [XmlRoot("routing")]
    public class ServiceRoutingConfig
    {
        [XmlArray("rules")]
        [XmlArrayItem("rule")]
        public ServiceRoutingRuleConfig[] Rules { get; set; } 
    }
}

