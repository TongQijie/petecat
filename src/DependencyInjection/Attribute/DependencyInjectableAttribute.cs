namespace Petecat.DependencyInjection.Attribute
{
    using System;

    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public class DependencyInjectableAttribute : Attribute
    {
        public Type Inference { get; set; }

        public bool Singleton { get; set; }

        public int Priority { get; set; }
    }
}