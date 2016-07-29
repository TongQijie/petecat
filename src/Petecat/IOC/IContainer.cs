using System;
using System.Collections.Generic;
using System.Reflection;

namespace Petecat.IOC
{
    public interface IContainer
    {
        IEnumerable<ITypeDefinition> LoadedTypeDefinitions { get; }

        object Resolve(Type targetType, params object[] arguments);

        T Resolve<T>(params object[] arguments);

        object Resolve(Type targetType);

        T Resolve<T>();

        object Resolve(string objectName);

        T Resolve<T>(string objectName);

        void RegisterContainerAssembly(Assembly assembly);

        void RegisterContainerAssembly(string assemblyPath);

        void RegisterContainerObjects(string objectsFile);

        bool ContainsTypeDefinition(Type targetType);

        bool TryGetTypeDefinition(Type targetType, out ITypeDefinition typeDefinition);

        bool TryGetTypeDefinition(string type, out ITypeDefinition typeDefinition);
    }
}
