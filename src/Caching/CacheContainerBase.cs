using System;
using System.Linq;
using System.Collections.Concurrent;

using Petecat.Extension;

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

        public bool Contains(Predicate<string> predicate)
        {
            return Items.Keys.ToArray().Exists(x => predicate(x));
        }

        public ICacheItem Get(Predicate<ICacheItem> predicate)
        {
            return Items.Values.ToList().FirstOrDefault(x => predicate(x));
        }

        public ICacheItem[] Get(Predicate<string> predicate)
        {
            var items = new ICacheItem[0];

            var keys = Items.Keys.ToList().Where(x => predicate(x));
            foreach (var key in keys)
            {
                ICacheItem value;
                if (Items.TryGetValue(key, out value))
                {
                    items = items.Append(value);
                }
            }

            return items;
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
