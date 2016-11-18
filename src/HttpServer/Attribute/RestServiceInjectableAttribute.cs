using Petecat.DependencyInjection.Attribute;

namespace Petecat.HttpServer.Attribute
{
    public class RestServiceInjectableAttribute : DependencyInjectableAttribute
    {
        public RestServiceInjectableAttribute()
        {
            Singleton = true;
        }

        public string ServiceName { get; set; }
    }
}