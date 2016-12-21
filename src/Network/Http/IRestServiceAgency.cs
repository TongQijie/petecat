using System.Collections.Generic;

namespace Petecat.Network.Http
{
    public interface IRestServiceAgency
    {
        TResponse Call<TResponse>(string resourceName, Dictionary<string, string> queryString);

        TResponse Call<TResponse>(string resourceName, object requestBody, Dictionary<string, string> queryString);

        TResponse Call<TResponse>(HttpVerb verb, string host, object requestBody, Dictionary<string, string> queryString);
    }
}
