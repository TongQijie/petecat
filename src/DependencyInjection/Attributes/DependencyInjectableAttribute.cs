using System;

namespace Petecat.DependencyInjection.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public class DependencyInjectableAttribute : Attribute
    {
        public Type Inference { get; set; }

        public bool Singleton { get; set; }

        public bool OverridedInference { get; protected set; }

        public bool TypeMatch { get; protected set; }

        public int Priority { get; set; }
    }
}