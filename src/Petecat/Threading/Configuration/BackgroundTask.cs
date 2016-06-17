using System.Xml.Serialization;

namespace Petecat.Threading.Configuration
{
    public class BackgroundTask
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("active")]
        public bool Active { get; set; }

        [XmlAttribute("provider")]
        public string Provider { get; set; }

        [XmlArray("arguments")]
        [XmlArrayItem("arg")]
        public BackgroundTaskArgument[] Arguments { get; set; }
    }
}
