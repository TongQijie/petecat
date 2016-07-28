using Petecat.Data.Formatters;
using Petecat.Extension;

using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Petecat.IOC
{
    public class AppDomainContainer : DefaultContainer
    {
        private static AppDomainContainer _Instance = null;

        public static AppDomainContainer Instance { get { return _Instance; } }

        public static AppDomainContainer Initialize()
        {
            if(_Instance == null)
            {
                _Instance = new AppDomainContainer();

                foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                {
                    _Instance.Register(assembly);
                }

                var directory = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);

                foreach (var assembly in directory.GetFiles("*.dll", SearchOption.AllDirectories).Select(x => Assembly.LoadFile(x.FullName)))
                {
                    _Instance.Register(assembly);
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
