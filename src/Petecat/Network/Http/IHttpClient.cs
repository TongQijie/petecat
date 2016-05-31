using System.Collections.Generic;

namespace Petecat.Network.Http
{
    public interface IHttpClient
    {
        Dictionary<string, string> RequestHeaders { get; }

        Dictionary<string, string> RequestQueryString { get; }

        TResponse Get<TResponse>();

        TResponse Post<TResponse>(object requestBody);
    }
}