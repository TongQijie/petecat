using Petecat.Formatter.Attributes;
using Petecat.Formatter.Json;

namespace Petecat.ConsoleApp.Formatter
{
    public class DurianClass
    {
        [JsonProperty(Alias = "name")]
        public string Name { get; set; }

        [JsonProperty(Alias = "age")]
        public int Age { get; set; }

        [JsonIngore]
        public string Gender { get; set; }

        [JsonProperty(Alias = "habbit")]
        public string[] Habbit { get; set; }

        [JsonProperty(Alias = "job")]
        public FilbertClass Filbert { get; set; }

        [JsonObject(Alias = "career")]
        public JsonObject Grapes { get; set; }
    }
}
