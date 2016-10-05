using System;

namespace Petecat.Caching
{
    internal class WritableCacheObject : CacheObjectBase, IWritableCacheObject
    {
        public WritableCacheObject(string key, Func<object, object> readSourceHandler, Action<object> updateSourceHandler)
            : base(key, readSourceHandler)
        {
            _UpdateSourceHandler = updateSourceHandler;
        }

        private Action<object> _UpdateSourceHandler = null;

        public void Flush()
        {
            if (_UpdateSourceHandler != null)
            {
                try
                {
                    _UpdateSourceHandler(_Value);
                }
                catch (Exception e)
                {
                    throw new Errors.CacheObjectValueWriteFailedException(Key, e);
                }
            }
        }
    }
}
