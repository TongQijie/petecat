using Petecat.Formatter;
using Petecat.DependencyInjection;

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
                throw new FileNotFoundException(Path);
            }

            Value = DependencyInjector.GetObject<IXmlFormatter>().ReadObject(ItemType, Path);

            return base.SetValue();
        }
    }
}
