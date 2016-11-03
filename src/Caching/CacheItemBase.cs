using System;

namespace Petecat.Caching
{
    public abstract class CacheItemBase : ICacheItem
    {
        public CacheItemBase(string key)
        {
            Key = key;
            CreationDate = DateTime.Now;
            ModifiedDate = DateTime.Now;
            IsDirty = true;
        }

        public string Key { get; private set; }

        public DateTime CreationDate { get; protected set; }

        public bool IsDirty { get; set; }

        public DateTime ModifiedDate { get; protected set; }

        protected object Value { get; set; }

        public virtual object GetValue()
        {
            if (IsDirty)
            {
                try
                {
                    IsDirty = !SetValue();
                }
                catch (Exception e)
                {
                    // TODO: throw
                }
            }

            return Value;
        }

        protected virtual bool SetValue()
        {
            ModifiedDate = DateTime.Now;
            return true;
        }
    }
}