using System;
using System.Linq;

using Petecat.Collection;
using Petecat.Utility;
using System.Reflection;

namespace Petecat.IOC
{
    public class DefaultContainer : IContainer
    {
        private ThreadSafeKeyedObjectCollection<string, ITypeDefinition> LoadedTypeDefinitions = new ThreadSafeKeyedObjectCollection<string, ITypeDefinition>();

        public object Resolve(Type targetType, params object[] arguments)
        {
            throw new NotImplementedException();
        }

        public T Resolve<T>(params object[] arguments)
        {
            throw new NotImplementedException();
        }

        public object AutoResolve(Type targetType)
        {
            ITypeDefinition typeDefinition;
            if (TryGetTypeDefinition(targetType, out typeDefinition))
            {
                var defaultConstructor = typeDefinition.GetConstructors().FirstOrDefault();

                foreach (var argument in defaultConstructor.Arguments)
                {
                    if (argument.ArgumentType.IsClass)
                    {
                        argument.ArgumentValue = AutoResolve(argument.ArgumentType);
                    }
                    else if (argument.ArgumentType.IsInterface)
                    {
                        var argumentTypeDefinition = LoadedTypeDefinitions.Values.Where(x => x.IsImplementInterface(argument.ArgumentType)).FirstOrDefault(x =>
                        {
                            Attributes.AutoResolvableAttribute attribute;
                            return ReflectionUtility.TryGetCustomAttribute<Attributes.AutoResolvableAttribute>(x.Type, y => y.SpecifiedType.Equals(argument.ArgumentType), out attribute);
                        });

                        if (argumentTypeDefinition == null)
                        {
                            return null;
                        }

                        argument.ArgumentValue = AutoResolve(argumentTypeDefinition.Type);
                    }
                    else
                    {
                        return null;
                    }
                }

                return typeDefinition.GetInstance(defaultConstructor.Arguments.Select(x => x.ArgumentValue));
            }

            return null;
        }

        public T AutoResolve<T>()
        {
            return (T)AutoResolve(typeof(T));
        }

        public bool ContainType(Type targetType)
        {
            return LoadedTypeDefinitions.Values.ToList().Exists(x => targetType.FullName == x.Key);
        }

        public bool TryGetTypeDefinition(Type targetType, out ITypeDefinition typeDefinition)
        {
            typeDefinition = LoadedTypeDefinitions.Values.FirstOrDefault(x => x.Key == targetType.FullName);
            return typeDefinition != null;
        }

        public void Register(params ITypeDefinition[] typeDefinitions)
        {
            if (typeDefinitions == null || typeDefinitions.Length == 0)
            {
                return;
            }

            typeDefinitions.Where(x => ReflectionUtility.ContainsCustomAttribute<Attributes.ResolvableAttribute>(x.Type)).ToList().ForEach(x => LoadedTypeDefinitions.Add(x));
        }
    }
}