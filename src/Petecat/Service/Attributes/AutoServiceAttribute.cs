using Petecat.IOC.Attributes;
using System;

namespace Petecat.Service.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class AutoServiceAttribute : AutoResolvableAttribute
    {
        public AutoServiceAttribute(Type specifiedType) 
            : base(specifiedType)
        {
        }

        public string ServiceName { get; set; }
    }
}
