using Petecat.DependencyInjection;
using Petecat.HttpServer.Attributes;
using Petecat.Utility;
using System;
using System.Reflection;

using Petecat.Extension;

namespace Petecat.HttpServer.DependencyInjection
{
    public class RestServiceTypeDefinition : TypeDefinitionBase
    {
        public RestServiceTypeDefinition(Type type)
        {
            Info = type;
            Attributes.RestServiceInjectableAttribute attribute;
            if (Reflector.TryGetCustomAttribute(type, null, out attribute))
            {
                Inference = attribute.Inference;
            }
            AssemblyInfo = new RestServiceAssemblyInfo(type.Assembly);
        }

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
