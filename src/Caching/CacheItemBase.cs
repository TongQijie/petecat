using System;

using Petecat.Logging;
using Petecat.DependencyInjection;

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
                        var retries = 3;
                        while(IsDirty && retries > 0)
                        {
                            try
                            {
                                IsDirty = !SetValue();
                            }
                            catch (Exception e)
                            {
                                if (retries > 1)
                                {
                                    Threading.ThreadBridging.Sleep(100);
                                }
                                else
                                {
                                    DependencyInjector.GetObject<IFileLogger>().LogEvent("CacheItemBase", Severity.Error, "failed to set value.", e);
                                }
                            }

                            retries--;
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