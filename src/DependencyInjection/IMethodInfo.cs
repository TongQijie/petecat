namespace Petecat.DependencyInjection
{
    public interface IMethodInfo : IInfo
    {
        IMethodDefinition MethodDefinition { get; }

        IParameterInfo[] ParameterInfos { get; }

        bool TryMatchParameters(object[] parameterValues, out object[] result);
    }
}