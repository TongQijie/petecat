using System;
using Petecat.DependencyInjection.Attributes;

namespace Petecat.HttpServer.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class RestServiceInjectableAttribute : DependencyInjectableAttribute
    {
        public string ServiceName { get; set; }
    }
}
