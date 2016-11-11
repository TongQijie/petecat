using System;

using Petecat.Utility;
using Petecat.Extension;

namespace Petecat.DependencyInjection
{
    public abstract class MethodInfoBase : IMethodInfo
    {
        public IMethodDefinition MethodDefinition { get; protected set; }

        public IParameterInfo[] ParameterInfos { get; protected set; }

        public ITypeDefinition TypeDefinition { get; protected set; }

        public bool TryMatchParameters(object[] parameterValues, out object[] result)
        {
            if (parameterValues == null || ParameterInfos == null)
            {
                // TODO: throw
            }

            result = new object[parameterValues.Length];

            if (parameterValues.Length != ParameterInfos.Length)
            {
                return false;
            }

            for (var i = 0; i < parameterValues.Length; i++)
            {
                var parameterInfo = ParameterInfos.FirstOrDefault(x => x.Index == i);
                if (parameterInfo == null)
                {
                    return false;
                }

                object o;
                if (!Converter.TryBeAssignable(parameterValues[i], parameterInfo.TypeDefinition.Info as Type, out o))
                {
                    return false;
                }

                result[i] = o;
            }

            return true;
        }
    }
}
