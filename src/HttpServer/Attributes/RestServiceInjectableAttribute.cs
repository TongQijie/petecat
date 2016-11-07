using System;
using Petecat.DependencyInjection.Attributes;

namespace Petecat.HttpServer.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class RestServiceInjectableAttribute : DependencyInjectableAttribute
    {
        public RestServiceInjectableAttribute()
        {
            TypeMatch = true;
        }

        public string ServiceName { get; set; }
    }
}
