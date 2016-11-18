namespace Petecat.HttpServer.Attribute
{
    using System;

    [AttributeUsage(AttributeTargets.Method)]
    public class RestServiceMethodAttribute : Attribute
    {
        public string MethodName { get; set; }

        public bool IsDefault { get; set; }
    }
}
