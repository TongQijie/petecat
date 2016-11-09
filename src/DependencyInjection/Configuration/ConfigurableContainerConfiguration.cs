using Petecat.Formatter.Attributes;

namespace Petecat.DependencyInjection.Configuration
{
    public class ConfigurableContainerConfiguration
    {
        [JsonProperty(Alias = "instances")]
        public InjectableInstanceConfiguration[] Instances { get; set; }
    }
}