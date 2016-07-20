namespace Petecat.IOC
{
    public interface IPropertyDefinition : IMemberDefinition
    {
        string PropertyName { get; }

        void SetValue(object instance, object value);
    }
}
