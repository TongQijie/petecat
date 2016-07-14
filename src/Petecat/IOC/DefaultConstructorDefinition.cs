using System.Reflection;
namespace Petecat.IOC
{
    public class DefaultConstructorDefinition : IConstructorDefinition
    {
        public DefaultConstructorDefinition(ConstructorInfo constructorInfo)
        {
            Info = constructorInfo;

            var parameters = constructorInfo.GetParameters();
            MethodArguments = new MethodArgument[parameters.Length];
            for (int i = 0; i < parameters.Length; i++)
            {
                MethodArguments[i] = new MethodArgument()
                {
                    Index = i,
                    Name = parameters[i].Name,
                    ArgumentType = parameters[i].ParameterType,
                };
            }
        }

        public MethodArgument[] MethodArguments { get; private set; }

        public MemberInfo Info { get; private set; }
    }
}
