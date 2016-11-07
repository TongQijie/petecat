using System;
using System.Collections.Concurrent;

using Petecat.Extension;

namespace Petecat.DependencyInjection
{
    public class ContainerBase : IContainer
    {
        private ConcurrentDictionary<string, IInstanceInfo> _RegisteredInstances = null;

        public ConcurrentDictionary<string, IInstanceInfo> RegisterInstances
        {
            get { return _RegisteredInstances ?? (_RegisteredInstances = new ConcurrentDictionary<string, IInstanceInfo>()); }
        }

        private ConcurrentDictionary<Type, ITypeDefinition> _RegisteredTypes = null;

        public ConcurrentDictionary<Type, ITypeDefinition> RegisteredTypes
        {
            get { return _RegisteredTypes ?? (_RegisteredTypes = new ConcurrentDictionary<Type, ITypeDefinition>()); }
        }

        public void RegisterInstance(IInstanceInfo instanceInfo)
        {
            if (!instanceInfo.Name.HasValue())
            {
                // TODO: throw
            }

            if (ContainsInstance(instanceInfo.Name))
            {
                return;
            }

            RegisterInstances.AddOrUpdate(instanceInfo.Name, instanceInfo, (a, b) => instanceInfo);
        }

        public void RegisterType(ITypeDefinition typeDefinition)
        {
            if (!(typeDefinition.Info is Type))
            {
                // TODO: throw
            }

            if (ContainsType(typeDefinition.Info as Type))
            {
                return;
            }

            RegisteredTypes.AddOrUpdate(typeDefinition.Info as Type, typeDefinition, (a, b) => typeDefinition);
        }

        public virtual object GetObject(Type objectType)
        {
            throw new NotImplementedException();
        }

        public virtual T GetObject<T>()
        {
            return (T)GetObject(typeof(T));
        }

        public virtual object GetObject(string objectName)
        {
            throw new NotImplementedException();
        }

        public virtual T GetObject<T>(string objectName)
        {
            return (T)GetObject(objectName);
        }

        public bool ContainsType(Type objectType)
        {
            return RegisteredTypes.ContainsKey(objectType);
        }

        public bool ContainsInstance(string objectName)
        {
            return RegisterInstances.ContainsKey(objectName);
        }
    }
}
