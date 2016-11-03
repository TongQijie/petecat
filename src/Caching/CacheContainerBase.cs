using System;
using System.Collections.Concurrent;
using System.Linq;

namespace Petecat.Caching
{
    public class CacheContainerBase : ICacheContainer
    {
        private ConcurrentDictionary<string, ICacheItem> _Items = null;

        public ConcurrentDictionary<string, ICacheItem> Items
        {
            get { return _Items ?? (_Items = new ConcurrentDictionary<string, ICacheItem>()); }
        }

        public void Add(ICacheItem item)
        {
            if (!Items.TryAdd(item.Key, item))
            {
                // TODO: throw
            }
        }

        public bool Contains(string key)
        {
            return Items.ContainsKey(key);
        }

        public ICacheItem Get(Predicate<ICacheItem> predicate)
        {
            return Items.Values.ToList().FirstOrDefault(x => predicate(x));
        }

        public ICacheItem Get(string key)
        {
            ICacheItem item;
            if (!Items.TryGetValue(key, out item))
            {
                // TODO: throw
            }

            return item;
        }

        public void Remove(string key)
        {
            ICacheItem item;
            if (!Items.TryRemove(key, out item))
            {
                // TODO: throw
            }
        }
    }
}
