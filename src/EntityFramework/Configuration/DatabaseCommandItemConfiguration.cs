using System.Data;
using System.Xml.Serialization;

namespace Petecat.EntityFramework.Configuration
{
    public class DatabaseCommandItemConfiguration
    {
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }

        [XmlAttribute(AttributeName = "database")]
        public string Database { get; set; }

        [XmlAttribute(AttributeName = "commandType")]
        public CommandType CommandType { get; set; }

        [XmlElement(ElementName = "commandText")]
        public string CommandText { get; set; }

        [XmlArray(ElementName = "params")]
        [XmlArrayItem(ElementName = "param")]
        public DatabaseCommandParameterConfiguration[] Parameters { get; set; }
    }
}