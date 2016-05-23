using System.Xml.Serialization;
using System.Linq;
using System;

namespace Petecat.Data.Configuration
{
    [XmlRoot("dataOperations")]
    public class DataOperationCollection
    {
        [XmlElement("dataOperation")]
        public DataOperation[] DataOperationCommands { get; set; }
    }
}