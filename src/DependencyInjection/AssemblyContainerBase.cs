using System;
using System.Collections.Concurrent;

namespace Petecat.DependencyInjection
{
    public class AssemblyContainerBase : ContainerBase, IAssemblyContainer
    {
        public object GetObject(Type targetType)
        {
            throw new NotImplementedException();
        }

        public T GetObject<T>()
        {
            throw new NotImplementedException();
        }

        public void RegisterAssembly(IAssemblyInfo assemblyInfo)
        {
            var typeDefinitions = assemblyInfo.GetTypeDefinitions();
            if (typeDefinitions != null && typeDefinitions.Length > 0)
            {
                foreach (var typeDefinition in typeDefinitions)
                {
                    RegisterType(typeDefinition);
                }
            }
        }

        private object InternalResolve(Type targetType)
        {
            throw new NotImplementedException();
        }
    }
}