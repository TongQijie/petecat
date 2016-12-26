#if !MONO

using System;
using System.Web;
using System.Linq;

using Petecat.Logging;
using Petecat.DependencyInjection;
using Petecat.HttpServer.DependencyInjection;

namespace Petecat.HttpServer
{
    public class WebSocketHandler : IHttpHandler
    {
        public WebSocketHandler(string serviceName)
        {
            ServiceName = serviceName;
        }

        public string ServiceName { get; private set; }

        public bool IsReusable { get { return false; } }

        public void ProcessRequest(HttpContext context)
        {
            if (context.IsWebSocketRequest)
            {
                try
                {
                    var container = DependencyInjector.GetContainer<HttpServerAssemblyContainer>();
                    if (container == null)
                    {
                        throw new Exception("http server container is null.");
                    }

                    var handler = GetExecutionHandler(container);
                    context.AcceptWebSocketRequest(handler.Process);
                }
                catch (Exception e)
                {
                    DependencyInjector.GetObject<IFileLogger>().LogEvent("RestServiceHttpHandler", Severity.Error, "failed to process websocket request.", e);
                }
            }
        }

        public IWebSocketExecutionHandler GetExecutionHandler(HttpServerAssemblyContainer container)
        {
            var typeDefinition = container.RegisteredTypes.Values.OfType<WebSocketTypeDefinition>()
               .FirstOrDefault(x => string.Equals(x.ServiceName, ServiceName, StringComparison.OrdinalIgnoreCase));
            if (typeDefinition == null)
            {
                throw new Exception(string.Format("websocket '{0}' cannot be found.", ServiceName));
            }

            return DependencyInjector.GetObject(typeDefinition.Info as Type) as IWebSocketExecutionHandler;
        }
    }
}

#endif