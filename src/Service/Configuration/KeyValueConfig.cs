using System.Xml.Serialization;
namespace Petecat.Service.Configuration
{
    public class KeyValueConfig
    {
        [XmlAttribute("key")]
        public string Key { get; set; }

        [XmlAttribute("value")]
        public string Value { get; set; }
    }
}
