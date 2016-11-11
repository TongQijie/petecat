using Petecat.Formatter;

using System;
using System.IO;

namespace Petecat.Caching
{
    public class JsonFileCacheItem : CacheItemBase, IFileCacheItem
    {
        public JsonFileCacheItem(string key, string path, Type itemType) : base(key)
        {
            Path = path;
            ItemType = itemType;
        }

        public string Path { get; private set; }

        public Type ItemType { get; private set; }

        protected override bool SetValue()
        {
            if (!File.Exists(Path))
            {
                // TODO: throw
            }

            Value = new JsonFormatter().ReadObject(ItemType, Path);

            return base.SetValue();
        }
    }
}
