namespace Petecat.DependencyInjection
{
    public interface IMethodInfo : IInfo
    {
        IParameterInfo[] ParameterInfos { get; }

        bool Match(object[] parameterValues);
    }
}