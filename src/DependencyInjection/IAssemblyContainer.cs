using System;
using System.Reflection;

namespace Petecat.DependencyInjection
{
    public interface IAssemblyContainer : IContainer
    {
        Assembly[] Assemblies { get; }

        void RegisterAssembly(IAssemblyInfo assemblyInfo);

        IAssemblyContainer RegisterAssemblies<T>() where T : IAssemblyInfo;

        bool CanInfer(Type targetType);
    }
}