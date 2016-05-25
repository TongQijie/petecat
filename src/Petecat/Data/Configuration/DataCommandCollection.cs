using System.Xml.Serialization;

namespace Petecat.Data.Configuration
{
    [XmlRoot("dataCommands")]
    public class DataCommandCollection
    {
        [XmlElement("dataCommand")]
        public DataCommand[] DataCommands { get; set; }
    }
}