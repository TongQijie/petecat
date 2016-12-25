using Petecat.Formatter.Attribute;

namespace Petecat.HttpServer.Configuration
{
    public class KeyValueItemConfiguration
    {
        [JsonProperty(Alias = "key")]
        public string Key { get; set; }

        [JsonProperty(Alias = "value")]
        public string Value { get; set; }
    }
}