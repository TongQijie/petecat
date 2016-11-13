using Petecat.Formatter.Json;
using Petecat.Formatter.Attribute;

namespace Petecat.DependencyInjection.Configuration
{
    public class InstanceParameterConfiguration
    {
        [JsonProperty(Alias = "name")]
        public string Name { get; set; }

        [JsonProperty(Alias = "index")]
        public int Index { get; set; }

        [JsonObject(Alias = "value")]
        public JsonObject Value { get; set; }
    }
}