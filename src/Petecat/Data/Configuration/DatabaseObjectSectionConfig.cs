using System.Xml.Serialization;

namespace Petecat.Data.Configuration
{
    [XmlRoot("databases")]
    public class DatabaseObjectSectionConfig
    {
        [XmlElement("database")]
        public DatabaseObjectConfig[] DatabaseObjects { get; set; }
    }
}
