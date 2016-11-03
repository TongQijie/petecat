using System;
namespace Petecat.HttpServer.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class RestServiceMethodAttribute : Attribute
    {
        public string MethodName { get; set; }

        public bool IsDefault { get; set; }
    }
}
