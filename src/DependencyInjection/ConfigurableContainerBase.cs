using Petecat.Caching;
using Petecat.Extending;
using Petecat.Configuring;
using Petecat.DependencyInjection.Configuration;

using System.Linq;
using System.Collections.Concurrent;

namespace Petecat.DependencyInjection
{
    public class ConfigurableContainerBase : ContainerBase, IConfigurableContainer
    {
        public override object GetObject(string objectName)
        {
            IInstanceInfo instanceInfo = null;
            if (!RegisterInstances.TryGetValue(objectName, out instanceInfo))
            {
                return null;
            }

            return instanceInfo.GetInstance();
        }

        public void RegisterConfigurableFile(IConfigurableFileInfo configurableFileInfo)
        {
            var configurer = DependencyInjector.GetObject<IStaticFileConfigurer>();
            if (configurer == null)
            {
                // TODO: throw
            }

            if (!configurer.ContainsKey(configurableFileInfo.Path))
            {
                configurer.Append(configurableFileInfo.Path, configurableFileInfo.Path, "json", 
                    typeof(ConfigurableContainerConfiguration), OnConfigurableFileChanged);
            }

            var instanceInfos = configurableFileInfo.GetInstanceInfos();
            if (instanceInfos != null && instanceInfos.Length > 0)
            {
                foreach (var instanceInfo in instanceInfos)
                {
                    ObjectMappings.AddOrUpdate(instanceInfo.Name, configurableFileInfo, (a, b) => configurableFileInfo);
                    RegisterInstance(instanceInfo);
                }
            }
        }

        private ConcurrentDictionary<string, IConfigurableFileInfo> _ObjectMappings = null;

        public ConcurrentDictionary<string, IConfigurableFileInfo> ObjectMappings
        {
            get { return _ObjectMappings ?? (_ObjectMappings = new ConcurrentDictionary<string, IConfigurableFileInfo>()); }
        }

        private void OnConfigurableFileChanged(ICacheItem item, bool dirty)
        {
            if (dirty)
            {
                var fileCacheItem = item as IFileCacheItem;
                if (fileCacheItem == null)
                {
                    return;
                }

                IInstanceInfo instanceInfo = null;
                IConfigurableFileInfo configurableFileInfo = null;
                
                foreach (var objectName in ObjectMappings.ToArray().Where(x => string.Equals(x.Value.Path, fileCacheItem.Path)).Select(x => x.Key))
                {
                    RegisterInstances.TryRemove(objectName, out instanceInfo);
                    
                    ObjectMappings.TryRemove(objectName, out configurableFileInfo);
                }

                if (configurableFileInfo != null)
                {
                    RegisterConfigurableFile(configurableFileInfo);
                }
            }
        }
    }
}
