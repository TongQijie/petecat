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
        public KeyValueItemConfiguration[] HttpApplicationRoutingConfiguration { get; set; }

        [JsonProperty(Alias = "resource")]
        public KeyValueItemConfiguration[] StaticResourceMappingConfiguration { get; set; }

        [JsonProperty(Alias = "headers")]
        public KeyValueItemConfiguration[] ResponseHeaders { get; set; }

        [JsonProperty(Alias = "rewrite")]
        public RewriteRuleConfiguration[] RewriteRules { get; set; }
    }
}