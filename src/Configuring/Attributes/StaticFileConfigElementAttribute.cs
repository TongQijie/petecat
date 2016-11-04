using Petecat.DependencyInjection.Attributes;
using System;

namespace Petecat.Configuring.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class StaticFileConfigElementAttribute : DependencyInjectableAttribute
    {
        public StaticFileConfigElementAttribute()
        {
            Sington = true;
        }

        public string Key { get; set; }

        public string Path { get; set; }

        public string FileFormat { get; set; }

        public Type ConfigurationType { get; set; }
    }
}