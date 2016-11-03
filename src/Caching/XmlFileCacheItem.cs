using Petecat.Data.Formatters;
using System;
using System.IO;

namespace Petecat.Caching
{
    public class XmlFileCacheItem : CacheItemBase, IFileCacheItem
    {
        public XmlFileCacheItem(string key, string path, Type itemType) : base(key)
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

            Value = new XmlFormatter().ReadObject(ItemType, Path);

            return base.SetValue();
        }
    }
}
