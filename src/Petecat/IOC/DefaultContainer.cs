using System;
using System.Linq;
using System.Collections.Generic;

using Petecat.Collection;
using Petecat.Utility;

namespace Petecat.IOC
{
    public class DefaultContainer : IContainer
    {
        private ThreadSafeKeyedObjectCollection<string, ITypeDefinition> _LoadedTypeDefinitions = new ThreadSafeKeyedObjectCollection<string, ITypeDefinition>();

        public IEnumerable<ITypeDefinition> LoadedTypeDefinitions { get { return _LoadedTypeDefinitions.Values; } }

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
            return InternalAutoResolve(targetType);
        }

        public T AutoResolve<T>()
        {
            return (T)InternalAutoResolve(typeof(T));
        }

        public bool ContainTypeDefinition(Type targetType)
        {
            return _LoadedTypeDefinitions.Values.ToList().Exists(x => targetType.FullName == x.Key);
        }

        public bool TryGetTypeDefinition(Type targetType, out ITypeDefinition typeDefinition)
        {
            typeDefinition = _LoadedTypeDefinitions.Values.FirstOrDefault(x => x.Key == targetType.FullName);
            return typeDefinition != null;
        }

        public void Register(params ITypeDefinition[] typeDefinitions)
        {
            if (typeDefinitions == null || typeDefinitions.Length == 0)
            {
                return;
            }

            typeDefinitions.Where(x => ReflectionUtility.ContainsCustomAttribute<Attributes.ResolvableAttribute>(x.Info)).ToList().ForEach(x => _LoadedTypeDefinitions.Add(x));
        }

        private object InternalAutoResolve(Type targetType)
        {
            if (targetType.IsClass)
            {
                ITypeDefinition typeDefinition;
                if (TryGetTypeDefinition(targetType, out typeDefinition))
                {
                    var defaultConstructor = typeDefinition.Constructors.FirstOrDefault();

                    foreach (var argument in defaultConstructor.MethodArguments)
                    {
                        argument.ArgumentValue = InternalAutoResolve(argument.ArgumentType);
                    }

                    return typeDefinition.GetInstance(defaultConstructor.MethodArguments.Select(x => x.ArgumentValue).ToArray());
                }

                return null;
            }
            else if (targetType.IsInterface)
            {
                var typeDefinition = _LoadedTypeDefinitions.Values.Where(x => x.IsImplementInterface(targetType)).FirstOrDefault(x =>
                {
                    Attributes.AutoResolvableAttribute attribute;
                    return ReflectionUtility.TryGetCustomAttribute<Attributes.AutoResolvableAttribute>(x.Info, y => y.SpecifiedType.FullName == targetType.FullName, out attribute);
                });

                if (typeDefinition == null)
                {
                    return null;
                }

                return InternalAutoResolve(typeDefinition.Info as Type);
            }
            else
            {
                return null;
            }
        }
    }
}