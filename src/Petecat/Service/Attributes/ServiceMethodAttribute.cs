using System;

namespace Petecat.Service.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ServiceMethodAttribute : Attribute
    {
        public ServiceMethodAttribute()
        {
        }

        public ServiceMethodAttribute(string name, ServiceAccessMethod method)
        {
            Name = name;
            Method = method;
        }

        public string Name { get; set; }

        public ServiceAccessMethod Method { get; set; }

        public bool IsDefaultMethod { get; set; }
    }
}
