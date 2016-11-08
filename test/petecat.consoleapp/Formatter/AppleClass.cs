using Petecat.Formatter.Attributes;
using System;

namespace Petecat.ConsoleApp.Formatter
{
    public class AppleClass
    {
        [JsonProperty(Alias = "id")]
        public string Id { get; set; }

        [JsonProperty(Alias = "creationDate")]
        public DateTime CreationDate { get; set; }

        [JsonProperty(Alias = "modifiedDate")]
        public DateTime ModifiedDate { get; set; }

        [JsonProperty(Alias = "title")]
        public string Title { get; set; }

        [JsonProperty(Alias = "abstract")]
        public string Abstract { get; set; }

        [JsonProperty(Alias = "content")]
        public string Content { get; set; }

        [JsonProperty(Alias = "signature")]
        public string Signature { get; set; }

        [JsonProperty(Alias = "deleted")]
        public bool Deleted { get; set; }
    }
}
