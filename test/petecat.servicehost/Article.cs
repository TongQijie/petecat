using Petecat.Data.Attributes;
namespace Petecat.ServiceHost
{
    public class Article
    {
        [JsonProperty(Alias = "name")]
        public string Name { get; set; }

        [JsonProperty(Alias = "snippet")]
        public string Snippet { get; set; }
    }
}