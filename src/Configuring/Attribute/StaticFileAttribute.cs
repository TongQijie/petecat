using Petecat.DependencyInjection.Attribute;

namespace Petecat.Configuring.Attribute
{
    public class StaticFileAttribute : DependencyInjectableAttribute
    {
        public StaticFileAttribute()
        {
            Singleton = true;
        }

        public string Key { get; set; }

        public string Path { get; set; }

        public string FileFormat { get; set; }

        public bool IsMultipleFiles { get; set; }
    }
}