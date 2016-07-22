using Petecat.IOC;
using Petecat.Utility;
using Petecat.Data.Formatters;
using Petecat.Logging;
using Petecat.Logging.Loggers;
using Petecat.Extension;

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
        public IContainer Container { get; private set; }

        public ServiceHttpApplication()
        {
            LoggerManager.SetLogger(new FileLogger(LoggerManager.AppDomainLoggerName, "./log".FullPath()));

            Container = new DefaultContainer();

            var configValue = AppConfigUtility.GetAppConfig("serviceContainer", "./Configuration/ServiceContainer.config");

            try
            {
                var serviceAssembliesConfig = new XmlFormatter().ReadObject<Configuration.ServiceAssembliesConfig>(configValue.FullPath(), Encoding.UTF8);

                if (serviceAssembliesConfig.ServiceAssemblies != null && serviceAssembliesConfig.ServiceAssemblies.Length > 0)
                {
                    foreach (var serviceAssemblyConfig in serviceAssembliesConfig.ServiceAssemblies)
                    {
                        var assembly = Assembly.LoadFile(serviceAssemblyConfig.Path.FullPath());
                        Container.Register(assembly.GetTypes().Select(x => new DefaultTypeDefinition(x)).ToArray());
                    }
                }
            }
            catch (Exception e)
            {
                LoggerManager.GetLogger().LogEvent("ServiceHttpApplication", LoggerLevel.Fatal, e);
                return;
            }

            ServiceManager.Instance = new ServiceManager(Container);
        }
    }
}
