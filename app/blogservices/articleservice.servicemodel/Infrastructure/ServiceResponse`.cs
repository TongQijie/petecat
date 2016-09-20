using System.Runtime.Serialization;

namespace ArticleService.ServiceModel.Infrastructure
{
    [DataContract]
    public class ServiceResponse<T> : ServiceResponse where T : class
    {
        [DataMember(Name = "body")]
        public T Body { get; set; }
    }
}