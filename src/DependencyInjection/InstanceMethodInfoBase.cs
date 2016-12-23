using System;
using System.Reflection;

using Petecat.Extending;

namespace Petecat.DependencyInjection
{
    public class InstanceMethodInfoBase : MethodInfoBase, IInstanceMethodInfo
    {
        public InstanceMethodInfoBase(ITypeDefinition typeDefinition, MethodInfo methodInfo)
        {
            TypeDefinition = typeDefinition;

            MethodName = methodInfo.Name;

            MethodDefinition = new InstanceMethodDefinitionBase(methodInfo);
        }

        public string MethodName { get; private set; }

        public object Invoke(object instance, params object[] parameters)
        {
            object[] result;
            if (!TryMatchParameters(parameters ?? new object[0], out result))
            {
                throw new Exception(string.Format("method '{0}' parameter is not valid.", MethodName));
            }

            return (MethodDefinition.Info as MethodInfo).Invoke(instance, result);
        }

        public override IParameterInfo[] ParameterInfos
        {
            get
            {
                if (_ParameterInfos == null)
                {
                    var methodInfo = MethodDefinition.Info as MethodInfo;

                    _ParameterInfos = new ParameterInfoBase[0];
                    foreach (var parameterInfo in methodInfo.GetParameters())
                    {
                        _ParameterInfos = _ParameterInfos.Append(new ParameterInfoBase(parameterInfo));
                    }
                }

                return _ParameterInfos;
            }
        }
    }
}
