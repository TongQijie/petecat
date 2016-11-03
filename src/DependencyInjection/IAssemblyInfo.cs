using System;
using System.Reflection;

namespace Petecat.DependencyInjection
{
    public interface IAssemblyInfo
    {
        Assembly Assembly { get; }

        ITypeDefinition[] GetTypeDefinitions();
    }
}
