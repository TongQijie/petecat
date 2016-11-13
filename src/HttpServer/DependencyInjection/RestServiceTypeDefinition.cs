using Petecat.Utility;
using Petecat.Extension;
using Petecat.DependencyInjection;
using Petecat.HttpServer.Attribute;

using System;

namespace Petecat.HttpServer.DependencyInjection
{
    public class RestServiceTypeDefinition : TypeDefinitionBase
    {
        public RestServiceTypeDefinition(Type type, string serviceName)
        {
            Info = type;
            RestServiceInjectableAttribute attribute;
            if (Reflector.TryGetCustomAttribute(type, null, out attribute))
            {
                Inference = attribute.Inference;
                Singleton = attribute.Singleton;
            }
            AssemblyInfo = new RestServiceAssemblyInfo(type.Assembly);
            ServiceName = serviceName;
        }

        public string ServiceName { get; set; }

        public override IInstanceMethodInfo[] InstanceMethods
        {
            get
            {
                if (_InstanceMethods == null)
                {
                    var instanceMethods = new IInstanceMethodInfo[0];

                    foreach (var methodInfo in (Info as Type).GetMethods())
                    {
                        RestServiceMethodAttribute attribute;
                        if (!Reflector.TryGetCustomAttribute(methodInfo, null, out attribute))
                        {
                            continue;
                        }

                        instanceMethods = instanceMethods.Append(new RestServiceInstanceMethodInfo(
                            this, 
                            methodInfo, 
                            attribute.MethodName.HasValue() ? attribute.MethodName : methodInfo.Name,
                            attribute.IsDefault));
                    }

                    _InstanceMethods = instanceMethods;
                }

                return _InstanceMethods;
            }
        }
    }
}
