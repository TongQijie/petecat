using Petecat.Formatter.Attribute;

namespace Petecat.Network.Http.Configuration
{
    public class RestServiceResourceConfiguration
    {
        [JsonProperty(Alias = "name")]
        public string Name { get; set; }

        [JsonProperty(Alias = "method")]
        public string Method { get; set; }

        [JsonProperty(Alias = "host")]
        public string Host { get; set; }
    }
}
