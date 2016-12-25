using Petecat.Configuring.Attribute;
using Petecat.Formatter.Attribute;

namespace Petecat.App.Url
{
    [StaticFile(
        Key = "UrlConfiguration",
        Path = "./Configuration/url.json",
        FileFormat = "json",
        Inference = typeof(IUrlConfiguration))]
    public class UrlConfiguration : IUrlConfiguration
    {
        [JsonProperty(Alias = "replacement")]
        public ReplacementConfiguration Replacement { get; set; }
    }
}
