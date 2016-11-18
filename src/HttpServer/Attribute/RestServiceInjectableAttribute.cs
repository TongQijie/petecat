using System;

using Petecat.DependencyInjection.Attribute;

namespace Petecat.HttpServer.Attribute
{
    [AttributeUsage(AttributeTargets.Class)]
    public class RestServiceInjectableAttribute : DependencyInjectableAttribute
    {
        public RestServiceInjectableAttribute()
        {
            TypeMatch = true;
            Singleton = true;
        }

        public string ServiceName { get; set; }
    }
}