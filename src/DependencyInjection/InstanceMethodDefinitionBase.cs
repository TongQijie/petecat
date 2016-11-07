using System.Reflection;

namespace Petecat.DependencyInjection
{
    public class InstanceMethodDefinitionBase : IMethodDefinition
    {
        public InstanceMethodDefinitionBase(MethodInfo methodInfo)
        {
            Info = methodInfo;
        }

        public MemberInfo Info { get; protected set; }
    }
}
