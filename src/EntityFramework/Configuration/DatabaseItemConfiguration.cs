using Petecat.Formatter.Attribute;

namespace Petecat.EntityFramework.Configuration
{
    public class DatabaseItemConfiguration
    {
        [JsonProperty(Alias = "name")]
        public string Name { get; set; }

        [JsonProperty(Alias = "connectionString")]
        public string ConnectionString { get; set; }

        [JsonProperty(Alias = "provider")]
        public string Provider { get; set; }
    }
}
