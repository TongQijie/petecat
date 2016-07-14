using System;
using System.Collections.Generic;

namespace Petecat.IOC
{
    public interface IContainer
    {
        IEnumerable<ITypeDefinition> LoadedTypeDefinitions { get; }

        object Resolve(Type targetType, params object[] arguments);

        T Resolve<T>(params object[] arguments);

        object AutoResolve(Type targetType);

        T AutoResolve<T>();

        void Register(params ITypeDefinition[] assemblies);

        bool ContainTypeDefinition(Type targetType);

        bool TryGetTypeDefinition(Type targetType, out ITypeDefinition typeDefinition);
    }
}
