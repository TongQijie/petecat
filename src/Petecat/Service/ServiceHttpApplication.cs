using Petecat.IOC;
using Petecat.Utility;
using Petecat.Data.Formatters;
using Petecat.Logging;
using Petecat.Logging.Loggers;

using System.Web;
using System;
using System.Reflection;
using System.Linq;
using System.IO;
using System.Text;

namespace Petecat.Service
{
    public class ServiceHttpApplication : HttpApplication
    {
        public ServiceHttpApplication()
        {
            LoggerManager.SetLogger(new FileLogger(LoggerManager.AppDomainLoggerName, GetFullPath("/log")));

            var container = new DefaultContainer();

            var serviceAssembliesConfigPath = AppConfigUtility.GetAppConfig("serviceAssemblies", string.Empty);
            if (string.IsNullOrEmpty(serviceAssembliesConfigPath))
            {
                LoggerManager.GetLogger().LogEvent("ServiceHttpApplication", LoggerLevel.Fatal, "config element 'serviceAssemblies' is missing.");
                return;
            }

            try
            {
                var serviceAssembliesConfig = new XmlFormatter().ReadObject<Configuration.ServiceAssembliesConfig>(GetFullPath(serviceAssembliesConfigPath), Encoding.UTF8);

                if (serviceAssembliesConfig.ServiceAssemblies != null && serviceAssembliesConfig.ServiceAssemblies.Length > 0)
                {
                    foreach (var serviceAssemblyConfig in serviceAssembliesConfig.ServiceAssemblies)
                    {
                        var assembly = Assembly.LoadFile(GetFullPath(serviceAssemblyConfig.Path));
                        container.Register(assembly.GetTypes().Select(x => new DefaultTypeDefinition(x)).ToArray());
                    }
                }
            }
            catch (Exception e)
            {
                LoggerManager.GetLogger().LogEvent("ServiceHttpApplication", LoggerLevel.Fatal, e);
                return;
            }

            ServiceManager.Instance = new ServiceManager(container);
        }

        private string GetFullPath(string path)
        {
            if (path.StartsWith("/"))
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path.TrimStart('/'));
            }
            else
            {
                return path;
            }
        }
    }
}
