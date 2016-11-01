namespace Petecat.DependencyInjection
{
    public interface IParameterInfo : IInfo
    {
        int Index { get; }

        string ParameterName { get; }

        object ParameterValue { get; }
    }
}
