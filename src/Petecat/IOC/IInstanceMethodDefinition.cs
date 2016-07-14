namespace Petecat.IOC
{
    public interface IInstanceMethodDefinition : IMemberDefinition
    {
        string MethodName { get; }

        bool IsMatch(MethodArgument[] arguments);

        bool TryGetMatchedArguments(MethodArgument[] arguments, out object[] matchedArguments);

        T Invoke<T>(object instance, params object[] paramaters);

        object Invoke(object instance, params object[] parameters);
    }
}
