using System;
using System.IO;
using System.Reflection;
using System.Runtime.Caching;
using System.Text;

namespace Petecat.Configuration
{
    public class FileConfigurationManagerBase : AbstractConfigurationManager, IFileConfigurationManager
    {
        public FileConfigurationManagerBase(bool enableCache)
            : base(enableCache)
        {
        }

        public override void Set(string key, object value)
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

            var value = Data.Xml.Serializer.ReadObject<T>(fileInfo.FullName, Encoding.UTF8);
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
                    try
                    {
                        this.LoadFromXml<T>(fileInfo.FullName, key);
                    }
                    catch (Exception e)
                    {
                        Logging.LoggerManager.Get().LogEvent(Assembly.GetExecutingAssembly().FullName, Logging.LoggerLevel.Error, string.Format("failed to load config file. path={0}", fileInfo.FullName), e);
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

        public void LoadFromIni(string filename, string key)
        {
            throw new NotImplementedException();
        }

        public void LoadFromJson<T>(string filename, string key) where T : class
        {
            throw new NotImplementedException();
        }
    }
}
