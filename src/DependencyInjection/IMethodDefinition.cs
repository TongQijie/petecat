namespace Petecat.DependencyInjection
{
    public interface IMethodDefinition : IDefinition
    {
        IParameterInfo[] ParameterInfos { get; }
    }
}