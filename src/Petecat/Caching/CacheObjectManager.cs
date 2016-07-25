using Petecat.Collection;
using Petecat.Data.Formatters;
using Petecat.Threading.Watcher;

using System;
using System.IO;
using System.Text;

namespace Petecat.Caching
{
    public class CacheObjectManager
    {
        private static CacheObjectManager _Instance = null;

        public static CacheObjectManager Instance { get { return _Instance ?? (_Instance = new CacheObjectManager()); } }

        private ThreadSafeKeyedObjectCollection<string, ICacheObject> _CacheObjects = new ThreadSafeKeyedObjectCollection<string, ICacheObject>();

        public ICacheObject Add(string key, Func<object> source)
        {
            return _CacheObjects.Add(new CacheObjectBase(key, source));
        }

        public void AddXml<T>(string key, string path, bool enableWatcher)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException(path);
            }

            var fileInfo = new FileInfo(path);

            CacheObjectManager.Instance.Add(key, () =>
            {
                return new XmlFormatter().ReadObject<T>(path, Encoding.UTF8);
            });

            if (enableWatcher)
            {
                FolderWatcherManager.Instance.GetOrAdd(fileInfo.Directory.FullName)
                    .SetFileChangedHandler(fileInfo.Name, (w) =>
                    {
                        CacheObjectManager.Instance.Get(key).IsDirty = true;
                    }).Start();
            }
        }

        public ICacheObject Get(string key)
        {
            return _CacheObjects.Get(key, null);
        }

        public T GetValue<T>(string key)
        {
            var cacheObject = _CacheObjects.Get(key, null);
            if (cacheObject == null)
            {
                return default(T);
            }

            return (T)cacheObject.GetValue();
        }
    }
}
