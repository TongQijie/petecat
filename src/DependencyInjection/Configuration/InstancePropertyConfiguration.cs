using Petecat.Formatter.Json;
using Petecat.Formatter.Attributes;

namespace Petecat.DependencyInjection.Configuration
{
    public class InstancePropertyConfiguration
    {
        [JsonProperty(Alias = "name")]
        public string Name { get; set; }

        [JsonObject(Alias = "value")]
        public JsonObject Value { get; set; }
    }
}
