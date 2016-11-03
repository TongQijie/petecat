namespace Petecat.DependencyInjection
{
    public interface IPropertyInfo : IInfo
    {
        IPropertyDefinition PropertyDefinition { get; }

        string PropertyName { get; }

        object PropertyValue { get; }

        void SetValue(object value);
    }
}
