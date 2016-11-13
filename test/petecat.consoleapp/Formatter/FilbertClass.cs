using Petecat.Formatter.Attribute;

namespace Petecat.ConsoleApp.Formatter
{
    public class FilbertClass
    {
        [JsonProperty(Alias = "name")]
        public string Name { get; set; }

        [JsonProperty(Alias = "location")]
        public string Location { get; set; }
    }
}
