using System.Runtime.Serialization;

namespace ArticleService.ServiceModel.Infrastructure
{
    [DataContract]
    public class ServiceRequest<T> : ServiceRequest where T : class
    {
        [DataMember(Name = "body")]
        public T Body { get; set; }
    }
}
