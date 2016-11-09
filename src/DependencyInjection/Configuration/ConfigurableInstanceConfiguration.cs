using Petecat.Data.Attributes;
namespace Petecat.DependencyInjection.Configuration
{
    public class ConfigurableInstanceConfiguration
    {
        [JsonProperty(Alias = "name")]
        public string Name { get; set; }

        [JsonProperty(Alias = "type")]
        public string Type { get; set; }

        [JsonProperty(Alias = "singleton")]
        public bool Singleton { get; set; }

        [JsonProperty(Alias = "parameters")]
        public InstanceParameterConfiguration[] Parameters { get; set; }

        [JsonProperty(Alias = "properties")]
        public InstancePropertyConfiguration[] Properties { get; set; }
    }
}