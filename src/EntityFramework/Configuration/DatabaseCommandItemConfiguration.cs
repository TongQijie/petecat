using System.Data;

using Petecat.Formatter.Attribute;

namespace Petecat.EntityFramework.Configuration
{
    public class DatabaseCommandItemConfiguration
    {
        [JsonProperty(Alias = "name")]
        public string Name { get; set; }

        [JsonProperty(Alias = "commandText")]
        public string CommandText { get; set; }

        [JsonProperty(Alias = "parameters")]
        public DatabaseCommandParameterConfiguration[] Parameters { get; set; }

        [JsonProperty(Alias = "database")]
        public string Database { get; set; }

        [JsonProperty(Alias = "commandType")]
        public CommandType CommandType { get; set; }

        [JsonProperty(Alias = "timeout")]
        public int TimeOut { get; set; }
    }
}
