using System;

namespace Petecat.DependencyInjection
{
    public interface IAssemblyContainer : IContainer
    {
        object GetObject(Type targetType);

        T GetObject<T>();

        void RegisterAssembly(IAssemblyInfo assemblyInfo);
    }
}