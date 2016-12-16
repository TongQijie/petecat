using Petecat.Caching;
using Petecat.Monitor;
using Petecat.Utility;
using Petecat.Extending;
using Petecat.Caching.Delegates;
using Petecat.DependencyInjection;
using Petecat.Configuring.Attribute;
using Petecat.DependencyInjection.Attribute;

using System;
using System.IO;
using System.Linq;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;

namespace Petecat.Configuring
{
    [DependencyInjectable(Inference = typeof(IStaticFileConfigurer), Singleton = true)]
    public class StaticFileConfigurer : IStaticFileConfigurer
    {
        private IFileSystemMonitor _FileSystemMonitor;

        private ICacheContainer _Container = null;

        public ICacheContainer Container { get { return _Container ?? (_Container = new CacheContainerBase()); } }

        public StaticFileConfigurer(IFileSystemMonitor fileSystemMonitor)
        {
            _FileSystemMonitor = fileSystemMonitor;
        }

        public void Append(string key, string path, string fileFormat, Type configurationType, 
            CacheItemDirtyChangedHandlerDelegate dirtyChanged = null)
        {
            // build a CacheItem and add to container
            ICacheItem item = null;
            if (fileFormat.EqualsWith("xml"))
            {
                item = new XmlFileCacheItem(key, path, configurationType);
            }
            else if (fileFormat.EqualsWith("json"))
            {
                item = new JsonFileCacheItem(key, path, configurationType);
            }
            else if (fileFormat.EqualsWith("text"))
            {
                item = new TextFileCacheItem(key, path);
            }
            else
            {
                // TODO: throw
            }

            if (dirtyChanged != null)
            {
                item.DirtyChanged += dirtyChanged;
            }

            Container.Add(item);

            // start file monitor
            _FileSystemMonitor.Add(this, path, OnFileChanged, null, null, null);
        }

        private void OnFileChanged(string path)
        {
            var item = Container.Get(x => x is IFileCacheItem 
                && string.Equals(path.Replace("\\", "/"), (x as IFileCacheItem).Path.Replace("\\", "/"), StringComparison.OrdinalIgnoreCase));
            if (item != null)
            {
                item.IsDirty = true;
            }
        }

        public void Remove(string key, CacheItemDirtyChangedHandlerDelegate dirtyChanged = null)
        {
            var item = Container.Get(key) as IFileCacheItem;
            if (item == null)
            {
                // TODO: throw
            }

            if (dirtyChanged != null)
            {
                item.DirtyChanged -= dirtyChanged;
            }

            // stop file monitor
            _FileSystemMonitor.Remove(this, item.Path, OnFileChanged, null, null, null);

            // remove CacheItem from container
            Container.Remove(key);
        }

        public object GetValue(string key)
        {
            return Container.Get(key).GetValue();
        }

        public T GetValue<T>()
        {
            EnsureExists(typeof(T));

            string key;
            if (CachedConfigurationTypes.TryGetValue(typeof(T), out key))
            {
                return (T)Container.Get(key).GetValue();
            }

            return default(T);
        }

        public T[] GetValues<T>()
        {
            EnsureExists(typeof(T));

            string key;
            if (CachedConfigurationTypes.TryGetValue(typeof(T), out key))
            {
                return Container.Get(x => x.StartsWith(key)).Select(x => (T)x.GetValue()).ToArray();
            }

            return new T[0];
        }

        private ConcurrentDictionary<Type, string> CachedConfigurationTypes = new ConcurrentDictionary<Type, string>();

        private void EnsureExists(Type configurationType)
        {
            if (CachedConfigurationTypes.ContainsKey(configurationType))
            {
                return;
            }

            var obj = DependencyInjector.GetObject(configurationType);
            if (obj == null)
            {
                // TODO: throw
            }

            StaticFileAttribute attribute;
            if (!Reflector.TryGetCustomAttribute(obj.GetType(), null, out attribute))
            {
                throw new Exception(string.Format("type '{0}' is not specified by StaticFileConfigElementAttribute.", obj.GetType()));
            }

            if (attribute.IsMultipleFiles)
            {
                var fullPath = attribute.Path.FullPath();

                var directory = fullPath.Folder();
                if (directory == null || !directory.IsFolder())
                {
                    // TODO: throw
                }

                var name = fullPath.Name().Replace("*", "\\S*");
                foreach (var fileInfo in new DirectoryInfo(directory).GetFiles())
                {
                    if (Regex.IsMatch(fileInfo.Name, name, RegexOptions.IgnoreCase))
                    {
                        Append(attribute.Key + "_" + fileInfo.Name, fileInfo.FullName, attribute.FileFormat, obj.GetType());
                    }
                }
            }
            else
            {
                Append(attribute.Key, attribute.Path.FullPath(), attribute.FileFormat, obj.GetType());
            }

            CachedConfigurationTypes.TryAdd(configurationType, attribute.Key);
        }

        public bool ContainsKey(string key)
        {
            return Container.Contains(key);
        }
    }
}
