using Petecat.IoC;
using Petecat.Logging;
using Petecat.Extension;
using Petecat.Logging.Loggers;

using System;
using System.Web;

namespace Petecat.Service
{
    public class ServiceHttpApplication : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            Initialize();
        }

        private void Initialize()
        {
            LoggerManager.SetLogger(new FileLogger(LoggerManager.AppDomainLoggerName, "./log".FullPath()));

            try
            {
                AppDomainIoCContainer.Initialize();
                ServiceManager.Instance = new ServiceManager(AppDomainIoCContainer.Instance);
            }
            catch (Exception e)
            {
                LoggerManager.GetLogger().LogEvent("ServiceHttpApplication", LoggerLevel.Fatal, e);
                return;
            }
        }
    }
}
