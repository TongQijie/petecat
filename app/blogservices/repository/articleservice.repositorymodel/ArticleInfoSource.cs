using System;
using System.Runtime.Serialization;

namespace ArticleService.RepositoryModel
{
    [DataContract]
    public class ArticleInfoSource
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "creationDate")]
        public DateTime CreationDate { get; set; }

        [DataMember(Name = "modifiedDate")]
        public DateTime ModifiedDate { get; set; }

        [DataMember(Name = "title")]
        public string Title { get; set; }

        [DataMember(Name = "abstract")]
        public string Abstract { get; set; }

        [DataMember(Name = "content")]
        public string Content { get; set; }

        [DataMember(Name = "signature")]
        public string Signature { get; set; }

        [DataMember(Name = "deleted")]
        public bool Deleted { get; set; }
    }
}
