using Petecat.Configuring;
using Petecat.Configuring.Attributes;
using Petecat.Data.Attributes;
namespace Petecat.ConsoleApp.Configuring
{
    [StaticFileConfigElement(
        Key = "banana", 
        Path = "./banana.json", 
        FileFormat = "json", 
        Inference = typeof(IBananaInterface))]
    public class BananaClass : StaticFileConfigInstanceBase, IBananaInterface
    {
        [JsonProperty(Alias = "name")]
        public string Name { get; set; }

        [JsonProperty(Alias = "age")]
        public int Age { get; set; }
    }
}
