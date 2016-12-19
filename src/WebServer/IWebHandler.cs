namespace Petecat.WebServer
{
    public interface IWebHandler
    {
        void ProcessRequest(WebContext context);
    }
}