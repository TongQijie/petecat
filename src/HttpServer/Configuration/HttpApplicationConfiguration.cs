using Petecat.Configuring;
using Petecat.Configuring.Attributes;
using Petecat.Data.Attributes;

namespace Petecat.HttpServer.Configuration
{
    [StaticFileConfigElement(
        Key = "Global_HttpApplicationConfiguration",
        Path = "./Configuration/application.json",
        FileFormat = "json",
        Inference = typeof(IHttpApplicationConfiguration))]
    public class HttpApplicationConfiguration : StaticFileConfigInstanceBase, IHttpApplicationConfiguration
    {
        [JsonProperty(Alias = "routing")]
        public KeyValueConfigurationItem[] HttpApplicationRoutingConfiguration { get; set; }

        [JsonProperty(Alias = "resource")]
        public KeyValueConfigurationItem[] StaticResourceMappingConfiguration { get; set; }
    }
}
