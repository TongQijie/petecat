using System.Xml.Serialization;

namespace Petecat.Data.Configuration
{
    public class DatabaseObjectConfig : Collection.IKeyedObject<string>
    {
        [XmlAttribute("name")]
        public string Key { get; set; }

        [XmlAttribute("connectionString")]
        public string ConnectionString { get; set; }

        [XmlAttribute("provider")]
        public string Provider { get; set; }
    }
}