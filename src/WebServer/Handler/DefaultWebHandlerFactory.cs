using Petecat.DependencyInjection.Attribute;
using System;

namespace Petecat.WebServer.WebHandler
{
    [DependencyInjectable(Inference = typeof(IWebHandlerFactory), Singleton = true)]
    public class DefaultWebHandlerFactory : IWebHandlerFactory
    {
        public IWebHandler GetHandler(WebContext context)
        {
            throw new NotImplementedException();
        }
    }
}