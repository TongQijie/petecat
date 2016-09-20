using System;
using System.Runtime.Serialization;

namespace ArticleService.ServiceModel
{
    [DataContract]
    public class ArticleInfo
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "creationDate")]
        public string CreationDate { get; set; }

        [DataMember(Name = "modifiedDate")]
        public string ModifiedDate { get; set; }

        [DataMember(Name = "title")]
        public string Title { get; set; }

        [DataMember(Name = "abstract")]
        public string Abstract { get; set; }

        [DataMember(Name = "content")]
        public string Content { get; set; }

        [DataMember(Name = "signature")]
        public string Signature { get; set; }
    }
}