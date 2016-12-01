using Petecat.DependencyInjection;
using Petecat.HttpServer.Attribute;
using Petecat.Utility;
using System;
namespace Petecat.HttpServer.DependencyInjection
{
    public class WebSocketTypeDefinition : TypeDefinitionBase
    {
        public WebSocketTypeDefinition(Type type, string serviceName)
        {
            Info = type;
            WebSocketInjectableAttribute attribute;
            if (Reflector.TryGetCustomAttribute(type, null, out attribute))
            {
                Inference = attribute.Inference;
                Singleton = attribute.Singleton;
                Priority = attribute.Priority;
            }
            AssemblyInfo = new WebSocketAssemblyInfo(type.Assembly);
            ServiceName = serviceName;
        }

        public string ServiceName { get; set; }
    }
}
