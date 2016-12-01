using Petecat.DependencyInjection.Attribute;
namespace Petecat.HttpServer.Attribute
{
    public class WebSocketInjectableAttribute : DependencyInjectableAttribute
    {
        public WebSocketInjectableAttribute()
        {
            Singleton = false;
        }

        public string ServiceName { get; set; }
    }
}
