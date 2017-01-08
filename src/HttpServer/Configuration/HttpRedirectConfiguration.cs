using Petecat.Formatter.Attribute;

namespace Petecat.HttpServer.Configuration
{
    public class HttpRedirectConfiguration
    {
        [JsonProperty(Alias = "pattern")]
        public string Pattern { get; set; }

        [JsonProperty(Alias = "redirect")]
        public string Redirect { get; set; }

        [JsonProperty(Alias = "mode")]
        public HttpRedirectMode Mode { get; set; }
    }
}