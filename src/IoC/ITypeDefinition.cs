using System;

using Petecat.Collection;

namespace Petecat.IoC
{
    public interface ITypeDefinition : IKeyedObject<string>, IMemberDefinition
    {
        string FullName { get; }

        object GetInstance(params object[] arguments);

        bool IsImplementInterface(Type interfaceType);

        IConstructorMethodDefinition[] Constructors { get; }

        IInstanceMethodDefinition[] InstanceMethods { get; }

        IPropertyDefinition[] Properties { get; }

        ContainerAssemblyInfo AssemblyInfo { get; }
    }
}