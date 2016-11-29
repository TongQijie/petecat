using System.Xml.Serialization;

using Petecat.Configuring.Attribute;

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
