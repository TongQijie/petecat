using System.Xml.Serialization;

namespace Petecat.Threading.Configuration
{
    public class BackgroundTaskArgument
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("value")]
        public string Value { get; set; }
    }
}