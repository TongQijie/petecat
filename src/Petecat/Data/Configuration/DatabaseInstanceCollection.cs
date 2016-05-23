using System.Xml.Serialization;

namespace Petecat.Data.Configuration
{
    [XmlRoot("databases")]
    public class DatabaseInstanceCollection
    {
        [XmlElement("database")]
        public DatabaseInstance[] DatabaseInstances { get; set; }
    }
}
