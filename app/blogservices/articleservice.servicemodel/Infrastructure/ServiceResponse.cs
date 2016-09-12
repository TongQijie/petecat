using Petecat.Extension;

namespace ArticleService.ServiceModel.Infrastructure
{
    public class ServiceResponse
    {
        public ServiceError[] Errors { get; set; }

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
