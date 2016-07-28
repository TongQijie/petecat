using System.IO;
using System.Reflection;

namespace Petecat.IOC
{
    public class AssemblyInfo
    {
        public AssemblyInfo(Assembly assembly)
        {
            Location = assembly.Location;
            var info = new FileInfo(Location);
            Name = info.Name.Remove(info.Name.Length - info.Extension.Length);
        }

        public string Location { get; private set; }

        public string Name { get; private set; }
    }
}