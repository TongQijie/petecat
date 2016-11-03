using Petecat.Data.Attributes;
namespace Petecat.HttpServer.Configuration
{
    public class HttpApplicationConfiguration
    {
        [JsonProperty(Alias = "httpApplicationRouting")]
        public KeyValueConfigurationItem[] HttpApplicationRoutingConfiguration { get; set; }

        [JsonProperty(Alias = "staticResourceMapping")]
        public KeyValueConfigurationItem[] StaticResourceMappingConfiguration { get; set; }
    }
}
