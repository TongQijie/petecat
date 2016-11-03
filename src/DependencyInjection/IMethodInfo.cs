namespace Petecat.DependencyInjection
{
    public interface IMethodInfo : IInfo
    {
        IMethodDefinition MethodDefinition { get; }

        IParameterInfo[] ParameterInfos { get; }

        bool Match(object[] parameterValues);
    }
}