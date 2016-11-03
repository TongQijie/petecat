using Petecat.DependencyInjection;
using Petecat.HttpServer.DependencyInjection;
using Petecat.Logging;
using Petecat.Logging.Loggers;
using System;
using System.Web;
using Petecat.Extension;

namespace Petecat.HttpServer
{
    public class HttpApplicationBase : HttpApplication
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
                DependencyInjector.Setup(new RestServiceAssemblyContainer());
            }
            catch (Exception e)
            {
                LoggerManager.GetLogger().LogEvent("HttpApplicationBase", LoggerLevel.Fatal, e);
                return;
            }
        }
    }
}
