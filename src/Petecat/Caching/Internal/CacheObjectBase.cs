using System;

namespace Petecat.Caching
{
    internal class CacheObjectBase : ICacheObject
    {
        public CacheObjectBase(string key, Func<object> source)
        {
            Key = key;
            _Source = source;
        }

        public string Key { get; private set; }

        private Func<object> _Source = null;

        private object _Value = null;

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
                _Value = _Source();
                IsDirty = false;
            }
            catch (Exception) { }

            return _Value;
        }
    }
}
