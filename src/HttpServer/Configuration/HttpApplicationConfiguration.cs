using Petecat.Configuring.Attributes;
using Petecat.Data.Attributes;

namespace Petecat.HttpServer.Configuration
{
    [StaticFileConfigElement(
        Key = "Global_HttpApplicationConfiguration",
        Path = "./httpApplicationConfiguration.json",
        FileFormat = "json",
        Inference = typeof(IHttpApplicationConfiguration))]
    public class HttpApplicationConfiguration
    {
        [JsonProperty(Alias = "httpApplicationRouting")]
        public KeyValueConfigurationItem[] HttpApplicationRoutingConfiguration { get; set; }

        [JsonProperty(Alias = "staticResourceMapping")]
        public KeyValueConfigurationItem[] StaticResourceMappingConfiguration { get; set; }
    }
}
