﻿using System;
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

            RegisterInstances.AddOrUpdate(instanceInfo.Name, instanceInfo, (a, b) => instanceInfo);
        }

        public void RegisterType(ITypeDefinition typeDefinition)
        {
            if (!(typeDefinition.Info is Type))
            {
                // TODO: throw
            }

            RegisteredTypes.AddOrUpdate(typeDefinition.Info as Type, typeDefinition, (a, b) => typeDefinition);
        }
    }
}
