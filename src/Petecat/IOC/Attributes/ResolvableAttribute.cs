using System;

namespace Petecat.IOC.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public class ResolvableAttribute : Attribute
    {
    }
}
