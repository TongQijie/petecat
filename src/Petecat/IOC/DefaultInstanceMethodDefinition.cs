using System.Reflection;
using System.Linq;
using System;
using System.Collections.Generic;

namespace Petecat.IOC
{
    public class DefaultInstanceMethodDefinition : AbstractMethodDefinition, IInstanceMethodDefinition
    {
        public DefaultInstanceMethodDefinition(MethodInfo methodInfo)
            : base(methodInfo)
        {
            Info = methodInfo;
        }

        public string MethodName { get { return Info.Name; } }

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