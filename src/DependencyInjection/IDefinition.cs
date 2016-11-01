using System.Reflection;

namespace Petecat.DependencyInjection
{
    public interface IDefinition
    {
        MemberInfo Info { get; }
    }
}
