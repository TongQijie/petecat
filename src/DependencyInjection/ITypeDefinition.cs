using System;

namespace Petecat.DependencyInjection
{
    public interface ITypeDefinition : IDefinition
    {
        Type Inference { get; }

        bool Sington { get; }

        IConstructorMethodInfo[] ConstructorMethods { get; }

        IInstanceMethodInfo[] InstanceMethods { get; }

        IPropertyInfo[] Properties { get; }

        IAssemblyInfo AssemblyInfo { get; }

        object GetInstance(params object[] parameters);
    }
}
