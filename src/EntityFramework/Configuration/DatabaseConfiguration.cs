using Petecat.Configuring;
using Petecat.Formatter.Attribute;
using Petecat.Configuring.Attribute;

namespace Petecat.EntityFramework.Configuration
{
    [StaticFileConfigElement(Inference = typeof(IDatabaseConfiguration),
        Key = "Global_DatabaseConfiguration",
        Path = "./configuration/database.json",
        FileFormat = "json")]
    public class DatabaseConfiguration : StaticFileConfigInstanceBase, IDatabaseConfiguration
    {
        [JsonProperty(Alias = "databases")]
        public DatabaseItemConfiguration[] Databases { get; set; }
    }
}
