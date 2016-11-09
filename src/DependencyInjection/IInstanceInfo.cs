namespace Petecat.DependencyInjection
{
    public interface IInstanceInfo : IInfo
    {
        string Name { get; }

        bool Singleton { get; }

        IParameterInfo[] ParameterInfos { get; }

        IPropertyInfo[] PropertyInfos { get; }

        object GetInstance();
    }
}