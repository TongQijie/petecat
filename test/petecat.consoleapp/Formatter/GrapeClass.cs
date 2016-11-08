
using Petecat.Formatter.Attributes;

namespace Petecat.ConsoleApp.Formatter
{
    public class GrapeClass
    {
        [JsonProperty(Alias = "from")]
        public string From { get; set; }

        [JsonProperty(Alias = "to")]
        public string To { get; set; }

        [JsonProperty(Alias = "company")]
        public string Company { get; set; }
    }
}
