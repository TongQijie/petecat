using System;

namespace Petecat.Caching.Errors
{
    public class CacheObjectValueUpdateFailedException : Exception
    {
        public CacheObjectValueUpdateFailedException(string cacheObjectName, Exception innerException)
            : base(string.Format("cache object '{0}' fails to update value.", cacheObjectName), innerException)
        {
        }
    }
}
