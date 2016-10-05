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

        public ICacheObject Add(string key, Func<object, object> readSourceHandler)
        {
            return _CacheObjects.Add(new CacheObjectBase(key, readSourceHandler));
        }

        public ICacheObject Add(string key, Func<object, object> readSourceHandler, Action<object> updateSourceHandler)
        {
            return _CacheObjects.Add(new WritableCacheObject(key, readSourceHandler, updateSourceHandler));
        }

        [Obsolete("this method is replaced by Add<T>(string key, string path, IObjectFormatter objectFormatter, bool enableWatcher)")]
        public void Add<T>(string key, string path, Encoding encoding, IObjectFormatter objectFormatter, bool enableWatcher)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException(path);
            }

            var fileInfo = new FileInfo(path);

            CacheObjectManager.Instance.Add(key, (v) => objectFormatter.ReadObject<T>(path));

            if (enableWatcher)
            {
                FolderWatcherManager.Instance.GetOrAdd(fileInfo.Directory.FullName)
                    .SetFileChangedHandler(fileInfo.Name, (w) =>
                    {
                        CacheObjectManager.Instance.GetObject(key).IsDirty = true;
                    }).Start();
            }
        }

        public void Add(string key, string path, Type objectType, IObjectFormatter objectFormatter, bool enableWatcher)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException(path);
            }

            var fileInfo = new FileInfo(path);

            CacheObjectManager.Instance.Add(key, (v) => objectFormatter.ReadObject(objectType, path));

            if (enableWatcher)
            {
                FolderWatcherManager.Instance.GetOrAdd(fileInfo.Directory.FullName)
                    .SetFileChangedHandler(fileInfo.Name, (w) =>
                    {
                        CacheObjectManager.Instance.GetObject(key).IsDirty = true;
                    }).Start();
            }
        }

        public void Add<T>(string key, string path, IObjectFormatter objectFormatter, bool enableWatcher)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException(path);
            }

            var fileInfo = new FileInfo(path);

            CacheObjectManager.Instance.Add(key, (v) => objectFormatter.ReadObject<T>(path));

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

        public bool Exists(string key)
        {
            return _CacheObjects.ContainsKey(key);
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
