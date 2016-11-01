using System;

namespace Petecat.Caching
{
    internal class CacheObjectBase : ICacheObject
    {
        public CacheObjectBase(string key, Func<object, object> readSourceHandler)
        {
            Key = key;
            _ReadSourceHandler = readSourceHandler;
        }

        public string Key { get; private set; }

        public string Path { get; set; }

        private Func<object, object> _ReadSourceHandler = null;

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
                _Value = _ReadSourceHandler(_Value);
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
