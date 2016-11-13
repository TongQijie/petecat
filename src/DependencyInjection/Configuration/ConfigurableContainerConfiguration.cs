using Petecat.Formatter.Attribute;

namespace Petecat.DependencyInjection.Configuration
{
    public class ConfigurableContainerConfiguration
    {
        [JsonProperty(Alias = "instances")]
        public ConfigurableInstanceConfiguration[] Instances { get; set; }
    }
}