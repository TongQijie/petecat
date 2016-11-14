using Petecat.Formatter.Attribute;

namespace Petecat.DependencyInjection.Configuration
{
    public class ConfigurableContainerConfiguration
    {
        [JsonProperty(Alias = "includes")]
        public IncludedContainerConfiguration[] Containers { get; set; }

        [JsonProperty(Alias = "instances")]
        public ConfigurableInstanceConfiguration[] Instances { get; set; }
    }
}