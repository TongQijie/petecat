using System;
using System.Reflection;

namespace Petecat.IOC
{
    public interface IContainer
    {
        object Resolve(Type targetType, params object[] arguments);

        T Resolve<T>(params object[] arguments);

        object AutoResolve(Type targetType);

        T AutoResolve<T>();

        void Register(params ITypeDefinition[] assemblies);
    }
}
