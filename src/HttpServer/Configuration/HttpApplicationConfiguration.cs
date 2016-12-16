using Petecat.Formatter.Attribute;
using Petecat.Configuring.Attribute;

namespace Petecat.HttpServer.Configuration
{
    [StaticFile(
        Key = "Global_HttpApplicationConfiguration",
        Path = "./Configuration/application.json",
        FileFormat = "json",
        Inference = typeof(IHttpApplicationConfiguration))]
    public class HttpApplicationConfiguration : IHttpApplicationConfiguration
    {
        [JsonProperty(Alias = "routing")]
        public KeyValueConfigurationItem[] HttpApplicationRoutingConfiguration { get; set; }

        [JsonProperty(Alias = "resource")]
        public KeyValueConfigurationItem[] StaticResourceMappingConfiguration { get; set; }

        [JsonProperty(Alias = "rewrite")]
        public KeyValueConfigurationItem[] RewriteRuleConfiguration { get; set; }
    }
}
