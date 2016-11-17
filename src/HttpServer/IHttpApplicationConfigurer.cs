namespace Petecat.HttpServer
{
    public interface IHttpApplicationConfigurer
    {
        string GetStaticResourceMapping(string key);

        string GetHttpApplicationRouting(string key);

        string ApplyRewriteRule(string url);
    }
}
