using System.Reflection;

using Petecat.Extension;

namespace Petecat.DependencyInjection
{
    public class DefaultConstructorMethodInfo : DefaultMethodInfoBase, IConstructorMethodInfo
    {
        public DefaultConstructorMethodInfo(ITypeDefinition typeDefinition, ConstructorInfo constructorInfo)
        {
            TypeDefinition = typeDefinition;

            ParameterInfos = new DefaultParameterInfo[0];
            foreach(var parameterInfo in constructorInfo.GetParameters())
            {
                ParameterInfos = ParameterInfos.Append(new DefaultParameterInfo(parameterInfo));
            }
        }
    }
}