using System.Reflection;

using Petecat.Extension;

namespace Petecat.DependencyInjection
{
    public class InstanceMethodInfoBase : MethodInfoBase, IInstanceMethodInfo
    {
        public InstanceMethodInfoBase(ITypeDefinition typeDefinition, MethodInfo methodInfo)
        {
            TypeDefinition = typeDefinition;

            MethodName = methodInfo.Name;

            MethodDefinition = new InstanceMethodDefinitionBase(methodInfo);

            ParameterInfos = new ParameterInfoBase[0];
            foreach (var parameterInfo in methodInfo.GetParameters())
            {
                ParameterInfos = ParameterInfos.Append(new ParameterInfoBase(parameterInfo));
            }
        }

        public string MethodName { get; private set; }

        public object Invoke(object instance, params object[] parameters)
        {
            if (!Match(parameters ?? new object[0]))
            {
                // TODO: throw
            }

            return (MethodDefinition.Info as MethodInfo).Invoke(instance, parameters);
        }
    }
}
