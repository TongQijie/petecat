using Petecat.DependencyInjection.Attributes;
using System;

namespace Petecat.Configuring.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class StaticFileConfigElementAttribute : DependencyInjectableAttribute
    {
        public StaticFileConfigElementAttribute()
        {
            Singleton = true;
        }

        public string Key { get; set; }

        public string Path { get; set; }

        public string FileFormat { get; set; }
    }
}