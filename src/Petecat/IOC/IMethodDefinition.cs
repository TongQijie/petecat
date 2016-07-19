using System.Collections.Generic;

namespace Petecat.IOC
{
    public interface IMethodDefinition : IMemberDefinition
    {
        IEnumerable<MethodArgument> MethodArguments { get; }

        bool IsMatch(MethodArgument[] arguments);

        bool TryGetArgumentValues(MethodArgument[] arguments, out object[] argumentValues);
    }
}
