using System;

using Petecat.Collection;

namespace Petecat.IOC
{
    public interface ITypeDefinition : IKeyedObject<string>, IMemberDefinition
    {
        object GetInstance(params object[] arguments);

        bool IsImplementInterface(Type interfaceType);

        IConstructorMethodDefinition[] Constructors { get; }

        IInstanceMethodDefinition[] InstanceMethods { get; }

        IPropertyDefinition[] Properties { get; }

        AssemblyInfo AssemblyInfo { get; }
    }
}