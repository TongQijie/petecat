using System;
using System.Reflection;

using Petecat.Extension;

namespace Petecat.DependencyInjection
{
    public class DefaultInstanceMethodInfo : DefaultMethodInfoBase, IInstanceMethodInfo
    {
        public DefaultInstanceMethodInfo(ITypeDefinition typeDefinition, MethodInfo methodInfo)
        {
            TypeDefinition = typeDefinition;

            MethodName = methodInfo.Name;

            ParameterInfos = new DefaultParameterInfo[0];
            foreach (var parameterInfo in methodInfo.GetParameters())
            {
                ParameterInfos = ParameterInfos.Append(new DefaultParameterInfo(parameterInfo));
            }
        }

        public string MethodName { get; private set; }

        public object Invoke(object instance, params object[] parameters)
        {
            if (!Match(parameters ?? new object[0]))
            {
                // TODO: throw
            }

            var methodInfo = instance.GetType().GetMethod(MethodName);
            if (methodInfo == null)
            {
                // TODO: throw
            }

            return methodInfo.Invoke(instance, parameters);
        }
    }
}
