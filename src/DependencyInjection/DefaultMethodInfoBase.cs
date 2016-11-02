using Petecat.Extension;
using Petecat.Utility;
using System;

namespace Petecat.DependencyInjection
{
    public class DefaultMethodInfoBase : IMethodInfo
    {
        public IParameterInfo[] ParameterInfos { get; protected set; }

        public ITypeDefinition TypeDefinition { get; protected set; }

        public bool Match(object[] parameterValues)
        {
            if (parameterValues == null || ParameterInfos == null)
            {
                // TODO: throw
            }

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
            }

            return true;
        }
    }
}
