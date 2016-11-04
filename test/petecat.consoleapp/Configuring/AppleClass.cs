using Petecat.Data.Attributes;
namespace Petecat.ConsoleApp.Configuring
{
    public class AppleClass
    {
        [JsonProperty(Alias = "name")]
        public string Name { get; set; }

        [JsonProperty(Alias = "age")]
        public int Age { get; set; }
    }
}
