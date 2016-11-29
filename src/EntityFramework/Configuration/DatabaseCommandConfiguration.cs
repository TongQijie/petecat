using Petecat.Configuring;
using Petecat.Formatter.Attribute;
using Petecat.Configuring.Attribute;

namespace Petecat.EntityFramework.Configuration
{
    [StaticFileConfigElement(Inference = typeof(IDatabaseCommandConfiguration),
        Key = "Global_DatabaseCommandConfiguration",
        Path = "./configuration/databaseCommand_*.json",
        FileFormat = "xml",
        IsMultipleFiles = true)]
    public class DatabaseCommandConfiguration : StaticFileConfigInstanceBase, IDatabaseCommandConfiguration
    {
        [JsonProperty(Alias = "commands")]
        public DatabaseCommandItemConfiguration[] Commands { get; set; }
    }
}
