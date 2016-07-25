using System;
using System.IO;
using System.Reflection;
using System.Runtime.Caching;
using System.Text;

namespace Petecat.Configuration
{
    [Obsolete("replaced by CacheObjectManager")]
    public class FileConfigurationManagerBase : AbstractConfigurationManager, IFileConfigurationManager
    {
        public FileConfigurationManagerBase(bool enableCache)
            : base(enableCache)
        {
        }

        public override void Set(string key, object value, CacheItemPolicy policy)
        {
            throw new NotSupportedException();
        }

        public void LoadFromXml<T>(string filename, string key) where T : class
        {
            if (!File.Exists(filename))
            {
                throw new FileNotFoundException();
            }

            var fileInfo = new FileInfo(filename);

            var value = new Data.Formatters.XmlFormatter().ReadObject<T>(fileInfo.FullName, Encoding.UTF8);
            if (value == null)
            {
                throw new FileLoadException();
            }

            if (EnableCache)
            {
                var policy = new CacheItemPolicy();
                policy.RemovedCallback = new CacheEntryRemovedCallback((args) =>
                {
                    if (args.RemovedReason == CacheEntryRemovedReason.ChangeMonitorChanged)
                    {
                        try
                        {
                            this.LoadFromXml<T>(fileInfo.FullName, key);
                        }
                        catch (Exception e)
                        {
                            Logging.LoggerManager.Get().LogEvent(Assembly.GetExecutingAssembly().FullName, Logging.LoggerLevel.Error, string.Format("failed to load config file. path={0}", fileInfo.FullName), e);
                        }
                    }
                });
                policy.ChangeMonitors.Add(new HostFileChangeMonitor(new string[] { fileInfo.FullName }));
                base.Set(key, value, policy);
            }
            else
            {
                base.Set(key, value, null);
            }
        }

        public void LoadFromIni<T>(string filename, string key)
        {
            if (!File.Exists(filename))
            {
                throw new FileNotFoundException();
            }

            var fileInfo = new FileInfo(filename);

            var value = Data.Ini.StringFormatter.ReadObject<T>(fileInfo.FullName, Encoding.UTF8, key);
            if (value == null)
            {
                throw new FileLoadException();
            }

            if (EnableCache)
            {
                if (_ObjectCache.Contains(key))
                {
                    _ObjectCache.Remove(key);
                }

                var policy = new CacheItemPolicy();
                policy.RemovedCallback = new CacheEntryRemovedCallback((args) =>
                {
                    if (args.RemovedReason == CacheEntryRemovedReason.ChangeMonitorChanged)
                    {
                        try
                        {
                            this.LoadFromIni<T>(fileInfo.FullName, key);
                        }
                        catch (Exception e)
                        {
                            Logging.LoggerManager.Get().LogEvent(Assembly.GetExecutingAssembly().FullName, Logging.LoggerLevel.Error, string.Format("failed to load config file. path={0}", fileInfo.FullName), e);
                        }
                    }
                });
                policy.ChangeMonitors.Add(new HostFileChangeMonitor(new string[] { fileInfo.FullName }));
                _ObjectCache.Add(key, value, policy);
            }
            else
            {
                if (_ConfigurationItems.ContainsKey(key))
                {
                    _ConfigurationItems.Remove(key);
                }

                _ConfigurationItems.Add(key, value);
            }
        }

        public void LoadFromJson<T>(string filename, string key) where T : class
        {
            throw new NotImplementedException();
        }
    }
}
