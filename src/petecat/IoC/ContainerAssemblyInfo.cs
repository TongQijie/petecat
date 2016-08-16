using System.IO;
using System.Reflection;

namespace Petecat.IoC
{
    public class ContainerAssemblyInfo
    {
        public ContainerAssemblyInfo(Assembly assembly)
        {
            Location = assembly.Location;
            var info = new FileInfo(Location);
            Name = info.Name.Remove(info.Name.Length - info.Extension.Length);
        }

        public string Location { get; private set; }

        public string Name { get; private set; }
    }
}