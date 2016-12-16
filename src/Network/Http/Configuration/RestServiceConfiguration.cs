using Petecat.Configuring.Attribute;
using Petecat.Formatter.Attribute;

namespace Petecat.Network.Http.Configuration
{
    [StaticFile(
        Key = "Global_RestServiceConfiguration",
        Path = "./configuration/restservice.json",
        FileFormat = "json",
        Inference = typeof(IRestServiceConfiguration))]
    public class RestServiceConfiguration : IRestServiceConfiguration
    {
        [JsonProperty(Alias = "resources")]
        public RestServiceResourceConfiguration[] ResourceConfiguration { get; set; }
    }
}