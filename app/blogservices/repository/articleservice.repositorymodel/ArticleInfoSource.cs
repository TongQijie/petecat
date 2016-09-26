using Petecat.Data.Attributes;
using System;
using System.Runtime.Serialization;

namespace ArticleService.RepositoryModel
{
    [DataContract]
    public class ArticleInfoSource
    {
        [DataMember(Name = "id")]
        [JsonProperty(Alias = "id")]
        public string Id { get; set; }

        [DataMember(Name = "creationDate")]
        [JsonProperty(Alias = "creationDate")]
        public DateTime CreationDate { get; set; }

        [DataMember(Name = "modifiedDate")]
        [JsonProperty(Alias = "modifiedDate")]
        public DateTime ModifiedDate { get; set; }

        [DataMember(Name = "title")]
        [JsonProperty(Alias = "title")]
        public string Title { get; set; }

        [DataMember(Name = "abstract")]
        [JsonProperty(Alias = "abstract")]
        public string Abstract { get; set; }

        [DataMember(Name = "content")]
        [JsonProperty(Alias = "content")]
        public string Content { get; set; }

        [DataMember(Name = "signature")]
        [JsonProperty(Alias = "signature")]
        public string Signature { get; set; }

        [DataMember(Name = "deleted")]
        [JsonProperty(Alias = "deleted")]
        public bool Deleted { get; set; }
    }
}
