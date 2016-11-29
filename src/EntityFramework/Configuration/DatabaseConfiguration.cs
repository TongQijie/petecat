using Petecat.Configuring;
using Petecat.Formatter.Attribute;
using Petecat.Configuring.Attribute;
using System.Xml.Serialization;

namespace Petecat.EntityFramework.Configuration
{
    [StaticFileConfigElement(Inference = typeof(IDatabaseConfiguration),
        Key = "Global_DatabaseConfiguration",
        Path = "./configuration/database.xml",
        FileFormat = "xml")]
    [XmlRoot(ElementName = "databases")]
    public class DatabaseConfiguration : IDatabaseConfiguration
    {
        [XmlElement(ElementName = "database")]
        public DatabaseItemConfiguration[] Databases { get; set; }
    }
}
