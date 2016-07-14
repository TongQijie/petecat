using System;

namespace Petecat.IOC.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class AutoResolvableAttribute : ResolvableAttribute
    {
        public AutoResolvableAttribute(Type specifiedType)
        {
            SpecifiedType = specifiedType;
        }

        public Type SpecifiedType { get; set; }
    }
}
