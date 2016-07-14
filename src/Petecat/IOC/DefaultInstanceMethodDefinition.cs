using System.Reflection;
using System.Linq;
namespace Petecat.IOC
{
    public class DefaultInstanceMethodDefinition : IInstanceMethodDefinition
    {
        public DefaultInstanceMethodDefinition(MethodInfo methodInfo)
        {
            Info = methodInfo;
        }

        public MemberInfo Info { get; private set; }

        public string MethodName { get { return Info.Name; } }

        public bool IsMatch(MethodArgument[] arguments)
        {
            foreach (var parameterInfo in (Info as MethodInfo).GetParameters())
            {
                var argument = arguments.FirstOrDefault(x => x.Name == parameterInfo.Name || x.Index == parameterInfo.Position);
                if (argument == null)
                {
                    return false;
                }
            }

            return true;
        }

        public bool TryGetMatchedArguments(MethodArgument[] arguments, out object[] matchedArguments)
        {
            matchedArguments = new object[0];

            foreach (var parameterInfo in (Info as MethodInfo).GetParameters().OrderBy(x => x.Position))
            {
                var argument = arguments.FirstOrDefault(x => x.Name == parameterInfo.Name || x.Index == parameterInfo.Position);
                if (argument == null)
                {
                    return false;
                }

                matchedArguments = matchedArguments.Concat(new object[] { argument.ArgumentValue }).ToArray();
            }

            return true;
        }

        public T Invoke<T>(object instance, params object[] paramaters)
        {
            return (T)(Info as MethodInfo).Invoke(instance, paramaters);
        }

        public object Invoke(object instance, params object[] parameters)
        {
            return (Info as MethodInfo).Invoke(instance, parameters);
        }
    }
}