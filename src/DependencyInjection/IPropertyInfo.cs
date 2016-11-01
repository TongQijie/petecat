namespace Petecat.DependencyInjection
{
    public interface IPropertyInfo : IInfo
    {
        string PropertyName { get; }

        object PropertyValue { get; }
    }
}
