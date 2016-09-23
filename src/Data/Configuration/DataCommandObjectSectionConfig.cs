using System.Xml.Serialization;

namespace Petecat.Data.Configuration
{
    [XmlRoot("dataCommands")]
    public class DataCommandObjectSectionConfig
    {
        [XmlElement("dataCommand")]
        public DataCommandObjectConfig[] DataCommands { get; set; }
    }
}