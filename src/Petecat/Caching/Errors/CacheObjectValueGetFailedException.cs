using System;

namespace Petecat.Caching.Errors
{
    public class CacheObjectValueGetFailedException : Exception
    {
        public CacheObjectValueGetFailedException(string cacheObjectName, Exception innerException)
            : base(string.Format("cache object '{0}' fails to get value.", cacheObjectName), innerException)
        {
        }
    }
}
