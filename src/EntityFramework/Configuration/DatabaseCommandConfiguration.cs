using Petecat.Configuring.Attribute;

using System.Xml.Serialization;

namespace Petecat.EntityFramework.Configuration
{
    [StaticFileConfigElement(Inference = typeof(IDatabaseCommandConfiguration),
        Key = "Global_DatabaseCommandConfiguration",
        Path = "./configuration/databaseCommands_*.xml",
        FileFormat = "xml",
        IsMultipleFiles = true)]
    [XmlRoot(ElementName = "databaseCommands")]
    public class DatabaseCommandConfiguration : IDatabaseCommandConfiguration
    {
        [XmlElement(ElementName = "command")]
        public DatabaseCommandItemConfiguration[] Commands { get; set; }
    }
}
