using System.Xml.Serialization;

namespace Petecat.Threading.Configuration
{
    public class TaskObjectConfig
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("description")]
        public string Description { get; set; }

        [XmlAttribute("type")]
        public string Type { get; set; }

        [XmlAttribute("active")]
        public bool Active { get; set; }
    }
}
