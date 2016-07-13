using System;

namespace Petecat.IOC.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class AutoResolvableAttribute : Attribute
    {
        public AutoResolvableAttribute(Type specifiedType)
        {
            SpecifiedType = specifiedType;
        }

        public Type SpecifiedType { get; set; }
    }
}
