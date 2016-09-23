using System;

namespace Petecat.Caching.Errors
{
    public class CacheObjectValueWriteFailedException : Exception
    {
        public CacheObjectValueWriteFailedException(string key, Exception innerException)
            : base(string.Format("cache object '{0}' fails to write value.", key), innerException)
        {
        }
    }
}
