using System;

namespace Petecat.Caching
{
    internal class CacheObjectBase : ICacheObject
    {
        public CacheObjectBase(string key, Func<object> readCacheHandler)
        {
            Key = key;
            _ReadCacheHandler = readCacheHandler;
        }

        public string Key { get; private set; }

        private Func<object> _ReadCacheHandler = null;

        protected object _Value = null;

        public bool IsDirty { get; set; }

        public object GetValue()
        {
            if (IsDirty || _Value == null)
            {
                return UpdateValue();
            }
            else
            {
                return _Value;
            }
        }

        private object UpdateValue()
        {
            try
            {
                _Value = _ReadCacheHandler();
                IsDirty = false;
            }
            catch (Exception e)
            {
                throw new Errors.CacheObjectValueUpdateFailedException(Key, e);
            }

            return _Value;
        }
    }
}
