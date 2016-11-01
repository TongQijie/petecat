namespace Petecat.DependencyInjection
{
    public interface IPropertyDefinition : IDefinition
    {
        void SetValue(object instance, object value);
    }
}
