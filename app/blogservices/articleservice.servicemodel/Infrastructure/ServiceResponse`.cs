namespace ArticleService.ServiceModel.Infrastructure
{
    public class ServiceResponse<T> : ServiceResponse where T : class
    {
        public T Body { get; set; }
    }
}