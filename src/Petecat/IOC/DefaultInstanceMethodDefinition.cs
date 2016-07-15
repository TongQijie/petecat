using System.Reflection;
using System.Linq;
using System;

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
                var argument = arguments.FirstOrDefault(x => x.Name.Equals(parameterInfo.Name, StringComparison.OrdinalIgnoreCase) 
                    || x.Index == parameterInfo.Position);
                if (argument == null)
                {
                    return false;
                }

                try
                {
                    Convert.ChangeType(argument.ArgumentValue, parameterInfo.ParameterType);
                }
                catch (Exception)
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
                var argument = arguments.FirstOrDefault(x => x.Name.Equals(parameterInfo.Name, StringComparison.OrdinalIgnoreCase) 
                    || x.Index == parameterInfo.Position);
                if (argument == null)
                {
                    return false;
                }

                try
                {
                    var argumentValue = Convert.ChangeType(argument.ArgumentValue, parameterInfo.ParameterType);
                    matchedArguments = matchedArguments.Concat(new object[] { argumentValue }).ToArray();
                }
                catch (Exception)
                {
                    return false;
                }
            }

            return true;
        }

        public T Invoke<T>(object instance, params object[] paramaters)
        {
            return (T)Invoke(instance, paramaters);
        }

        public object Invoke(object instance, params object[] parameters)
        {
            if (Info.ReflectedType.IsInterface)
            {
                return instance.GetType().GetMethod(Info.Name, parameters.Select(x => x.GetType()).ToArray()).Invoke(instance, parameters);
            }
            else
            {
                return (Info as MethodInfo).Invoke(instance, parameters);
            }
        }
    }
}