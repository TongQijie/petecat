namespace ArticleService.ServiceModel.Infrastructure
{
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

        public string Code { get; set; }

        public string Message { get; set; }
    }
}
