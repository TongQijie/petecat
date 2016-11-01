using System;
namespace Petecat.Service.Errors
{
    public class StaticResourceNotSupportedException : Exception
    {
        public StaticResourceNotSupportedException(string resourceType)
            : base(string.Format("static resource type '{0}' does not support.", resourceType))
        {
        }
    }
}
