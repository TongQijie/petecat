using Petecat.Logging;
using Petecat.DependencyInjection;
using Petecat.HttpServer.DependencyInjection;

using System;
using System.Web;
using Petecat.DependencyInjection.Attribute;
using Petecat.Configuring.Attribute;
using Petecat.DynamicProxy.DependencyInjection;

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
            try
            {
                DependencyInjector.Setup(new HttpServerAssemblyContainer());
                DependencyInjector.GetContainer<HttpServerAssemblyContainer>().RegisterAssemblies<AssemblyInfoBase<DependencyInjectableAttribute>>();
                DependencyInjector.GetContainer<HttpServerAssemblyContainer>().RegisterAssemblies<AssemblyInfoBase<StaticFileConfigElementAttribute>>();
                DependencyInjector.GetContainer<HttpServerAssemblyContainer>().RegisterAssemblies<RestServiceAssemblyInfo>();
                DependencyInjector.GetContainer<HttpServerAssemblyContainer>().RegisterAssemblies<WebSocketAssemblyInfo>();
                DependencyInjector.GetContainer<HttpServerAssemblyContainer>().RegisterAssemblies<DynamicProxyAssemblyInfo>();
            }
            catch (Exception e)
            {
                DependencyInjector.GetObject<IFileLogger>().LogEvent("HttpApplicationBase", Severity.Fatal, e);
                return;
            }
        }
    }
}
