namespace Petecat.DependencyInjection
{
    public interface IInstanceInfo : IInfo
    {
        IParameterInfo[] ParameterInfos { get; }

        IPropertyInfo[] PropertyInfos { get; }

        object GetInstance();
    }
}