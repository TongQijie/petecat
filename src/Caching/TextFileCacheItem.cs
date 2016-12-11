using System.IO;
using System.Text;

using Petecat.Extending;

namespace Petecat.Caching
{
    public class TextFileCacheItem : CacheItemBase, IFileCacheItem
    {
        public TextFileCacheItem(string key, string path) : base(key)
        {
            Path = path;
        }

        public string Path { get; private set; }

        protected override bool SetValue()
        {
            if (!Path.IsFile())
            {
                throw new FileNotFoundException(Path);
            }

            using (var inputStream = new StreamReader(Path, Encoding.UTF8))
            {
                Value = inputStream.ReadToEnd();
            }

            return base.SetValue();
        }
    }
}
