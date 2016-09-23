using System.Data;
using System.Xml.Serialization;

namespace Petecat.Data.Configuration
{
    public class DataCommandObjectParameterConfig
    {
        public DataCommandObjectParameterConfig()
        {
            Direction = ParameterDirection.Input;
            Size = -1;
        }

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("dbType")]
        public DbType DbType { get; set; }

        [XmlAttribute("direction")]
        public ParameterDirection Direction { get; set; }

        [XmlAttribute("size")]
        public int Size { get; set; }

        [XmlAttribute("scale")]
        public byte Scale { get; set; }
    }
}