namespace Petecat.DependencyInjection
{
    public interface IInstanceMethodInfo : IMethodInfo
    {
        string MethodName { get; }

        object Invoke(object instance, params object[] parameters);
    }
}
