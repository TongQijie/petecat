using System;

using Petecat.IOC.Attributes;

namespace Petecat.Service.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ServiceAttribute : ResolvableAttribute
    {
        public string ServiceName { get; set; }
    }
}
