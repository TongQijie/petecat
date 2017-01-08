using System.Collections.Generic;

namespace Petecat.HttpServer
{
    public interface IHttpApplicationConfigurer
    {
        string GetStaticResourceMapping(string key);

        string GetHttpApplicationRouting(string key);

        string ApplyRewriteRule(string url);

        Dictionary<string, string> GetReponseHeaders();

        string ApplyHttpRedirect(string url);
    }
}
