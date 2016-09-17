using System.Runtime.Serialization;

namespace ArticleService.ServiceModel.Infrastructure
{
    [DataContract]
    public class ServiceError
    {
        public ServiceError()
        {
        }

        public ServiceError(string code, string message)
        {
            Code = code;
            Message = message;
        }

        [DataMember(Name = "code")]
        public string Code { get; set; }

        [DataMember(Name = "message")]
        public string Message { get; set; }
    }
}
