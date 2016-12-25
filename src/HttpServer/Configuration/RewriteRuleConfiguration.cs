using Petecat.Formatter.Attribute;

namespace Petecat.HttpServer.Configuration
{
    public class RewriteRuleConfiguration
    {
        [JsonProperty(Alias = "pattern")]
        public string Pattern { get; set; }

        [JsonProperty(Alias = "value")]
        public string Value { get; set; }

        [JsonProperty(Alias = "mode")]
        public RewriteRuleMode Mode { get; set; }
    }
}