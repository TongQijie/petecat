using Petecat.Collection;
using Petecat.Data.Formatters;
using Petecat.Threading.Watcher;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Petecat.Caching
{
    public class CacheObjectManager
    {
        private static CacheObjectManager _Instance = null;

        public static CacheObjectManager Instance { get { return _Instance ?? (_Instance = new CacheObjectManager()); } }

        private ThreadSafeKeyedObjectCollection<string, ICacheObject> _CacheObjects = new ThreadSafeKeyedObjectCollection<string, ICacheObject>();

        public ICacheObject[] CacheObjects { get { return _CacheObjects.Values.ToArray(); } }

        public ICacheObject Add(string key, Func<object> readCacheHandler)
        {
            return _CacheObjects.Add(new CacheObjectBase(key, readCacheHandler));
        }

        public ICacheObject Add(string key, Func<object> readCacheHandler, Action<object> writeCacheHandler)
        {
            return _CacheObjects.Add(new WritableCacheObject(key, readCacheHandler, writeCacheHandler));
        }

        public void Add<T>(string key, string path, Encoding encoding, IObjectFormatter objectFormatter, bool enableWatcher)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException(path);
            }

            var fileInfo = new FileInfo(path);

            CacheObjectManager.Instance.Add(key, () => objectFormatter.ReadObject<T>(path, encoding));

            if (enableWatcher)
            {
                FolderWatcherManager.Instance.GetOrAdd(fileInfo.Directory.FullName)
                    .SetFileChangedHandler(fileInfo.Name, (w) =>
                    {
                        CacheObjectManager.Instance.GetObject(key).IsDirty = true;
                    }).Start();
            }
        }

        public void Remove(string key)
        {
            var cacheObject = _CacheObjects.Get(key, null);
            if (cacheObject != null)
            {
                _CacheObjects.Remove(cacheObject);
            }
        }

        public ICacheObject GetObject(string key)
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
