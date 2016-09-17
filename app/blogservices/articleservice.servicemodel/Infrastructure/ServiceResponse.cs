using Petecat.Extension;
using System.Runtime.Serialization;

namespace ArticleService.ServiceModel.Infrastructure
{
    [DataContract]
    public class ServiceResponse
    {
        [DataMember(Name = "errors")]
        public ServiceError[] Errors { get; set; }

        [DataMember(Name = "paging")]
        public Paging Paging { get; set; }

        public bool HasError
        {
            get { return Errors != null && Errors.Length > 0; }
        }

        public void AppendError(string code, string message)
        {
            if (Errors == null)
            {
                Errors = new ServiceError[0];
            }
            Errors = Errors.Append(new ServiceError(code, message));
        }
    }
}
