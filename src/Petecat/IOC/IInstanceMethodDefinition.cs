namespace Petecat.IOC
{
    public interface IInstanceMethodDefinition : IMethodDefinition
    {
        string MethodName { get; }

        T Invoke<T>(object instance, params object[] paramaters);

        object Invoke(object instance, params object[] parameters);
    }
}
