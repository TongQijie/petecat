namespace ArticleService.ServiceModel.Infrastructure
{
    public class ServiceRequest<T> : ServiceRequest where T : class
    {
        public T Body { get; set; }
    }
}
