using System.Reflection;

using Petecat.Extending;

namespace Petecat.DependencyInjection
{
    public class ConstructorMethodInfoBase : MethodInfoBase, IConstructorMethodInfo
    {
        public ConstructorMethodInfoBase(ITypeDefinition typeDefinition, ConstructorInfo constructorInfo)
        {
            TypeDefinition = typeDefinition;
            MethodDefinition = new ConstructorMethodDefinitionBase(constructorInfo);

            ParameterInfos = new ParameterInfoBase[0];
            foreach(var parameterInfo in constructorInfo.GetParameters())
            {
                ParameterInfos = ParameterInfos.Append(new ParameterInfoBase(parameterInfo));
            }
        }
    }
}