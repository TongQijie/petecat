namespace Petecat.HttpServer.Attribute
{
    using System;

    [AttributeUsage(AttributeTargets.Parameter)]
    public class ParameterInfoAttribute : Attribute
    {
        public ParameterSource Source { get; set; }

        public string Alias { get; set; }
    }
}