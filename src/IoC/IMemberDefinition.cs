using System.Reflection;

namespace Petecat.IoC
{
    public interface IMemberDefinition
    {
        MemberInfo Info { get; }
    }
}
