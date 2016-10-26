using System;
using System.IO;

using Petecat.Caching;
using Petecat.Extension;
using Petecat.Data.Formatters;
using Petecat.Threading.Watcher;
using Petecat.Configuring.Configuration;

namespace Petecat.Configuring
{
    internal class ConfigurationManager : IConfigurationManager
    {
        private static IConfigurationManager _Instance = null;

        public static IConfigurationManager Instance
        {
            get
            {
                return _Instance ?? (_Instance = new ConfigurationManager());
            }
        }

        private const string CacheObjectName = "Global_ConfigurationItems";

        private ConfigurationManager()
        {
            Initialize();
        }

        public T GetValue<T>(string key)
        {
            var items = CacheObjectManager.Instance.GetValue<ConfigurationItemsConfig>(CacheObjectName);
            if (items == null)
            {
                return default(T);
            }

            if (items.Items.Exists(x => string.Equals(key, x.Key, StringComparison.OrdinalIgnoreCase)))
            {
                return CacheObjectManager.Instance.GetValue<T>(key);
            }
            else 
            {
                return default(T);
            }
        }

        private void Initialize()
        {
            var path = "./Configuration/petecat.config".FullPath();

            if (!File.Exists(path))
            {
                return;
            }

            var fileInfo = new FileInfo(path);

            CacheObjectManager.Instance.Add(CacheObjectName, (v) =>
            {
                var items = new XmlFormatter().ReadObject<ConfigurationItemsConfig>(path);

                if (v == null)
                {
                    foreach (var item in items.Items)
                    {
                        AddConfiguration(item);
                    }
                }
                else
                {
                    var olderItems = v as ConfigurationItemsConfig;

                    foreach (var item in items.Items)
                    {
                        var olderItem = olderItems.Items.FirstOrDefault(x => string.Equals(x.Key, item.Key, StringComparison.OrdinalIgnoreCase));
                        if (olderItem == null)
                        {
                            AddConfiguration(item);
                        }
                        else if (!string.Equals(olderItem.Path, item.Path, StringComparison.OrdinalIgnoreCase)
                              || !string.Equals(olderItem.Type, item.Type, StringComparison.OrdinalIgnoreCase))
                        {
                            UpdateConfiguration(item);
                        }
                    }

                    foreach (var olderItem in olderItems.Items)
                    {
                        if (!items.Items.Exists(x => string.Equals(x.Key, olderItem.Key, StringComparison.OrdinalIgnoreCase)))
                        {
                            RemoveConfiguration(olderItem);
                        }
                    }
                }

                return items;
            });

            FolderWatcherManager.Instance.GetOrAdd(fileInfo.Directory.FullName)
                .SetFileChangedHandler(fileInfo.Name, (w) =>
                {
                    CacheObjectManager.Instance.GetObject(CacheObjectName).IsDirty = true;
                }).Start();
        }

        private void AddConfiguration(ConfigurationItemConfig item)
        {
            Type configurationType;
            if (!TryGetConfigurationType(item, out configurationType))
            {
                throw new Exception();
            }

            CacheObjectManager.Instance.Add(item.Key,
                                            item.Path.FullPath(), 
                                            configurationType,
                                            ObjectFormatterFactory.GetFormatter(ObjectFormatterType.Xml), 
                                            true);
        }

        private void UpdateConfiguration(ConfigurationItemConfig item)
        {
            RemoveConfiguration(item);

            AddConfiguration(item);
        }

        private void RemoveConfiguration(ConfigurationItemConfig item)
        {
            CacheObjectManager.Instance.Remove(item.Key);
        }

        private bool TryGetConfigurationType(ConfigurationItemConfig item, out Type configurationType)
        {
            if (!File.Exists(item.Path.FullPath()))
            {
                configurationType = null;
                return false;
            }

            return Utility.ReflectionUtility.TryGetType(item.Type, out configurationType);
        }
    }
}

