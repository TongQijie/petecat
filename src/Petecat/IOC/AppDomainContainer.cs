using Petecat.Data.Formatters;
using Petecat.Extension;
using System;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Petecat.IOC
{
    public class AppDomainContainer : DefaultContainer
    {
        private static AppDomainContainer _Instance = null;

        public static AppDomainContainer Instance { get { return _Instance; } }

        public static AppDomainContainer Initialize(string path = null)
        {
            if(_Instance == null)
            {
                _Instance = new AppDomainContainer();

                foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                {
                    _Instance.Register(assembly);
                }

                if (!string.IsNullOrEmpty(path))
                {
                    var containerAssembliesConfig = new XmlFormatter().ReadObject<Configuration.ContainerAssembliesConfig>(path, Encoding.UTF8);

                    foreach (var containerAssemblyConfig in containerAssembliesConfig.Assemblies.Where(x => !string.IsNullOrEmpty(x.Path)))
                    {
                        _Instance.Register(Assembly.LoadFile(containerAssemblyConfig.Path.FullPath()));
                    }
                }
            }

            return _Instance;
        }

        public void Register(Assembly assembly)
        {
            Register(assembly.GetTypes().ToList().Select(x => new DefaultTypeDefinition(x)).ToArray());
        }
    }
}
