using System;
using System.Collections.Generic;

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

        void Register(params ITypeDefinition[] typeDefinitions);

        void Register(string configFile);

        bool ContainsTypeDefinition(Type targetType);

        bool TryGetTypeDefinition(Type targetType, out ITypeDefinition typeDefinition);
    }
}
