using Petecat.Configuring.Attribute;
using Petecat.Formatter.Attribute;

namespace Files
{
    [StaticFile(
        Key = "AppConfiguration",
        Path = "./Configuration/app.json",
        FileFormat = "json",
        Inference = typeof(IAppConfiguration))]
    public class AppConfiguration : IAppConfiguration
    {
        [JsonProperty(Alias = "replacement")]
        public ReplacementConfiguration Replacement { get; set; }
    }
}