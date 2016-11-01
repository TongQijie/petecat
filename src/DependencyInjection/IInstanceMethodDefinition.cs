namespace Petecat.DependencyInjection
{
    public interface IInstanceMethodDefinition : IDefinition
    {
        object Invoke(object instance, params object[] parameters);
    }
}
