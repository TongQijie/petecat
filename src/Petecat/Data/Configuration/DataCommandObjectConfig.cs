using System.Data;
using System.Xml.Serialization;

namespace Petecat.Data.Configuration
{
    public class DataCommandObjectConfig : Collection.IKeyedObject<string>
    {
        public DataCommandObjectConfig()
        {
            CommandType = CommandType.Text;
            TimeOut = 300;
        }

        [XmlAttribute("name")]
        public string Key { get; set; }

        [XmlElement("commandText")]
        public string CommandText { get; set; }

        [XmlArray("params")]
        [XmlArrayItem(ElementName = "param")]
        public DataCommandObjectParameterConfig[] Parameters { get; set; }

        [XmlAttribute("database")]
        public string Database { get; set; }
        
        [XmlAttribute("commandType")]
        public CommandType CommandType { get; set; }
        
        [XmlAttribute("timeOut")]
        public int TimeOut { get; set; }
    }
}