namespace Petecat.HttpServer.Attribute
{
    using System;

    [AttributeUsage(AttributeTargets.Method)]
    public class RestServiceMethodAttribute : Attribute
    {
        public string MethodName { get; set; }

        public bool IsDefault { get; set; }

        public RestServiceDataFormat RequestDataFormat { get; set; }

        public RestServiceDataFormat ResponseDataFormat { get; set; }

        public HttpVerb HttpVerb { get; set; }
    }
}
