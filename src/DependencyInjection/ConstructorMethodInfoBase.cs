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
        }

        public override IParameterInfo[] ParameterInfos
        {
            get
            {
                if (_ParameterInfos == null)
                {
                    var constructorInfo = MethodDefinition.Info as ConstructorInfo;

                    _ParameterInfos = new ParameterInfoBase[0];

                    foreach (var parameterInfo in constructorInfo.GetParameters())
                    {
                        _ParameterInfos = _ParameterInfos.Append(new ParameterInfoBase(parameterInfo));
                    }
                }

                return _ParameterInfos;
            }
        }
    }
}