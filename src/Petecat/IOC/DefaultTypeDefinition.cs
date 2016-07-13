using System;
using System.Linq;

using Petecat.Collection;

namespace Petecat.IOC
{
    public class DefaultTypeDefinition : ITypeDefinition
    {
        public DefaultTypeDefinition(Type type)
        {
            Type = type;
        }

        public string Key { get { return Type.FullName; } }

        public Type Type { get; private set; }

        public object GetInstance(params object[] arguments)
        {
            return Activator.CreateInstance(Type, arguments);
        }

        public MethodArguments[] GetConstructors()
        {
            var methodArgumentsArray = new MethodArguments[0];

            var constructors = Type.GetConstructors();
            foreach (var constructor in constructors)
            {
                var constructorParameters = constructor.GetParameters();
                if (constructorParameters == null)
                {
                    continue;
                }

                var methodArguments = new MethodArguments();
                methodArguments.Arguments = new MethodArgument[constructorParameters.Length];

                for (int i = 0; i < constructorParameters.Length; i++)
                {
                    methodArguments.Arguments[i].Index = i;
                    methodArguments.Arguments[i].Name = constructorParameters[i].Name;
                    methodArguments.Arguments[i].ArgumentType = constructorParameters[i].ParameterType;
                }

                methodArgumentsArray = methodArgumentsArray.Concat(new MethodArguments[] { methodArguments }).ToArray();
            }

            return methodArgumentsArray;
        }

        public bool IsImplementInterface(Type interfaceType)
        {
            return interfaceType.IsInterface && interfaceType.IsAssignableFrom(Type);
        }
    }
}