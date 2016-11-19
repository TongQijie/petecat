using Petecat.DependencyInjection.Attribute;
using System;

namespace Petecat.Configuring.Attribute
{
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