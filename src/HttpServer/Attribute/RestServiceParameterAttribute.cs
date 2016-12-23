namespace Petecat.HttpServer.Attribute
{
    using System;

    [AttributeUsage(AttributeTargets.Parameter)]
    public class RestServiceParameterAttribute : Attribute
    {
        public RestServiceParameterSource Source { get; set; }

        public string Alias { get; set; }
    }
}