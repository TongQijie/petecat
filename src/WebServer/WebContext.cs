namespace Petecat.WebServer
{
    public class WebContext
    {
        public WebContext(WebRequest request, WebResponse response)
        {
            Request = request;
            Response = response;
        }

        public WebRequest Request { get; private set; }

        public WebResponse Response { get; private set; }
    }
}
