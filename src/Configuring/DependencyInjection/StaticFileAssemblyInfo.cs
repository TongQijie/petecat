using System.Reflection;

using Petecat.DependencyInjection;
using Petecat.Configuring.Attribute;

namespace Petecat.Configuring.DependencyInjection
{
    public class StaticFileAssemblyInfo : AssemblyInfoBase<StaticFileAttribute>
    {
        public StaticFileAssemblyInfo(Assembly assembly) : base(assembly) { }
    }
}