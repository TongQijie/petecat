namespace Petecat.WebServer
{
    public interface IWebHandlerFactory
    {
        IWebHandler GetHandler(WebContext context);
    }
}