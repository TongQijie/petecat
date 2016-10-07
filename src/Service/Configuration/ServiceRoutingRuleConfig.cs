using System.Xml.Serialization;

namespace Petecat
{
    public class ServiceRoutingRuleConfig
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("value")]
        public string Value { get; set; }

        [XmlAttribute("enabled")]
        public bool Enabled { get; set; }

        [XmlAttribute("description")]
        public string Description { get; set; }
    }
}

