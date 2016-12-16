using Petecat.DependencyInjection;
using Petecat.Extending;
using Petecat.HttpServer.Attribute;
using Petecat.Utility;
using System.Reflection;

namespace Petecat.HttpServer.DependencyInjection
{
    public class WebSocketAssemblyInfo : AssemblyInfoBase<WebSocketInjectableAttribute>
    {
        public WebSocketAssemblyInfo(Assembly assembly)
            : base(assembly)
        {
        }

        public override ITypeDefinition[] GetTypeDefinitions()
        {
            var typeDefinitions = new ITypeDefinition[0];

            foreach (var type in Assembly.GetTypes().Subset(x => x.IsClass))
            {
                WebSocketInjectableAttribute attribute;
                if (Reflector.TryGetCustomAttribute(type, x => x.GetType().Equals(typeof(WebSocketInjectableAttribute)), out attribute))
                {
                    typeDefinitions = typeDefinitions.Append(new WebSocketTypeDefinition(type, attribute.ServiceName));
                }
            }

            return typeDefinitions;
        }
    }
}