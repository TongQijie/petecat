using System;

namespace Petecat.DependencyInjection.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public class DependencyInjectableAttribute : Attribute
    {
        public Type Inference { get; set; }

        public bool Sington { get; set; }
    }
}
