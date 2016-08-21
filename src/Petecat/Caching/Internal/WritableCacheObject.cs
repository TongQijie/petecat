using System;

namespace Petecat.Caching
{
    internal class WritableCacheObject : CacheObjectBase, IWritableCacheObject
    {
        public WritableCacheObject(string key, Func<object> readCacheHandler, Action<object> writeCacheHandler)
            : base(key, readCacheHandler)
        {
            _WriteCacheHandler = writeCacheHandler;
        }

        private Action<object> _WriteCacheHandler = null;

        public void Flush()
        {
            if (_WriteCacheHandler != null)
            {
                try
                {
                    _WriteCacheHandler(_Value);
                }
                catch (Exception e)
                {
                    throw new Errors.CacheObjectValueWriteFailedException(Key, e);
                }
            }
        }
    }
}
