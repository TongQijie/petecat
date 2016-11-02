namespace Petecat.DependencyInjection
{
    public interface ITypeDefinition : IDefinition
    {
        IConstructorMethodDefinition[] ConstructorMethods { get; }

        IInstanceMethodDefinition[] InstanceMethods { get; }

        IPropertyDefinition[] Properties { get; }

        IAssemblyInfo AssemblyInfo { get; }

        object GetInstance(params object[] parameters);
    }
}
