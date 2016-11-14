using Petecat.Formatter.Attribute;

namespace Petecat.DependencyInjection.Configuration
{
    public class IncludedContainerConfiguration
    {
        [JsonProperty(Alias = "path")]
        public string RelativePath { get; set; }
    }
}
