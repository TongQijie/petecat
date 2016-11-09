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
        }

        public string Key { get; private set; }

        public DateTime CreationDate { get; protected set; }

        private bool _IsDirty = true;

        public bool IsDirty
        {
            get
            {
                return _IsDirty;
            }
            set
            {
                if (_IsDirty != value)
                {
                    _IsDirty = value;

                    if (DirtyChanged != null)
                    {
                        DirtyChanged.Invoke(this, value);
                    }
                }
            }
        }

        public event Delegates.CacheItemDirtyChangedHandlerDelegate DirtyChanged;

        public DateTime ModifiedDate { get; protected set; }

        protected object Value { get; set; }

        private object _SetValueLocker = new object();

        public virtual object GetValue()
        {
            if (IsDirty)
            {
                lock (_SetValueLocker)
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