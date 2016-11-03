using System.Reflection;

namespace Petecat.DependencyInjection
{
    public class ConstructorMethodDefinitionBase : IMethodDefinition
    {
        public ConstructorMethodDefinitionBase(ConstructorInfo constructorInfo)
        {
            Info = constructorInfo;
        }

        public MemberInfo Info { get; protected set; }
    }
}
