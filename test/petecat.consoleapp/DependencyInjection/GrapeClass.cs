using Petecat.Formatter.Attribute;

namespace Petecat.ConsoleApp.DependencyInjection
{
    public class GrapeClass
    {
        public GrapeClass() { }

        public GrapeClass(int count)
        {
            Count = count;
        }

        [JsonProperty(Alias = "count")]
        public int Count { get; set; }
    }
}
