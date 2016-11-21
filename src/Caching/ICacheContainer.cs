using System;

namespace Petecat.Caching
{
    public interface ICacheContainer
    {
        void Add(ICacheItem item);

        void Remove(string key);

        ICacheItem Get(string key);

        ICacheItem Get(Predicate<ICacheItem> predicate);

        ICacheItem[] Get(Predicate<string> predicate);

        bool Contains(string key);

        bool Contains(Predicate<string> predicate);
    }
}
