namespace Petecat.Service.Client
{
    public abstract class AbstractServiceClient
    {
        public AbstractServiceClient(string resourceName)
        {
            ResourceName = resourceName;
        }

        public string ResourceName { get; private set; }

        public abstract TResponse Call<TResponse>();

        public abstract TResponse Call<TResponse>(object requestBody);
    }
}
