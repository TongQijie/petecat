using Petecat.Formatter.Attribute;
using System.Data;

namespace Petecat.EntityFramework.Configuration
{
    public class DatabaseCommandParameterConfiguration
    {
        [JsonProperty(Alias = "name")]
        public string Name { get; set; }

        [JsonProperty(Alias = "type")]
        public DbType DbType { get; set; }

        [JsonProperty(Alias = "direction")]
        public ParameterDirection Direction { get; set; }

        [JsonProperty(Alias = "size")]
        public int Size { get; set; }
    }
}
