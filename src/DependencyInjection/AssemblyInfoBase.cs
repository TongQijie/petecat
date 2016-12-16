using System.Reflection;

using Petecat.DependencyInjection.Attribute;

namespace Petecat.DependencyInjection
{
    public class AssemblyInfoBase : AssemblyInfoBase<DependencyInjectableAttribute>
    {
        public AssemblyInfoBase(Assembly assembly) : base(assembly)
        {
        }
    }
}