namespace Petecat.Network.Http
{
    public interface IRestServiceHttpClient
    {
        TResponse Call<TResponse>(string resourceName);

        TResponse Call<TResponse>(string resourceName, object requestBody);

        TResponse Call<TResponse>(HttpVerb verb, string host, object requestBody);
    }
}
