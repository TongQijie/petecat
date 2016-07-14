namespace Petecat.IOC
{
    public interface IConstructorDefinition : IMemberDefinition
    {
        MethodArgument[] MethodArguments { get; }
    }
}