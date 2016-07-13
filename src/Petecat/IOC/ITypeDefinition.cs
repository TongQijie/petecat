using Petecat.Collection;
using System;

namespace Petecat.IOC
{
    public interface ITypeDefinition : IKeyedObject<string>
    {
        object GetInstance(params object[] arguments);

        MethodArguments[] GetConstructors();

        bool IsImplementInterface(Type interfaceType);

        Type Type { get; }
    }
}