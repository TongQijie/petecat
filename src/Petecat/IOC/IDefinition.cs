using System.Reflection;

namespace Petecat.IOC
{
    public interface IMemberDefinition
    {
        MemberInfo Info { get; }
    }
}
