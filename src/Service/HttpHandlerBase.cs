namespace Petecat.Service
{
    public class HttpHandlerBase
    {
        public HttpHandlerBase(string handledUrl)
        {
            HandledUrl = handledUrl;
        }

        public string HandledUrl { get; private set; }
    }
}