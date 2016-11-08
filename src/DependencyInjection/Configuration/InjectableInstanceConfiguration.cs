using Petecat.Data.Attributes;
namespace Petecat.DependencyInjection.Configuration
{
    public class InjectableInstanceConfiguration
    {
        [JsonProperty(Alias = "name")]
        public string Name { get; set; }

        [JsonProperty(Alias = "type")]
        public string Type { get; set; }

        [JsonProperty(Alias = "sington")]
        public bool Sington { get; set; }

        [JsonProperty(Alias = "parameters")]
        public InstanceParameterConfiguration[] Parameters { get; set; }

        [JsonProperty(Alias = "properties")]
        public InstancePropertyConfiguration[] Properties { get; set; }
    }
}