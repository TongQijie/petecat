using Petecat.Configuring;
using Petecat.Configuring.Attribute;
using Petecat.Formatter.Attribute;

namespace Petecat.Network.Http.Configuration
{
    [StaticFileConfigElement(
        Key = "Global_RestServiceConfiguration",
        Path = "./configuration/restservice.json",
        FileFormat = "json",
        Inference = typeof(IRestServiceConfiguration))]
    public class RestServiceConfiguration : StaticFileConfigInstanceBase, IRestServiceConfiguration
    {
        [JsonProperty(Alias = "resources")]
        public RestServiceResourceConfiguration[] ResourceConfiguration { get; set; }
    }
}