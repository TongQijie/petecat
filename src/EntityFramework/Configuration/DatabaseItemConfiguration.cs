using Petecat.Formatter.Attribute;
using System.Xml.Serialization;

namespace Petecat.EntityFramework.Configuration
{
    public class DatabaseItemConfiguration
    {
        [XmlAttribute(AttributeName = "database")]
        public string Name { get; set; }

        [XmlAttribute(AttributeName = "connectionString")]
        public string ConnectionString { get; set; }

        [XmlAttribute(AttributeName = "provider")]
        public string Provider { get; set; }
    }
}
