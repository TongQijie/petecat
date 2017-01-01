using Petecat.Configuring.DependencyInjection;
using Petecat.DependencyInjection;
using Petecat.DependencyInjection.Containers;
using System;

namespace Files
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args == null || args.Length != 3)
            {
                Console.WriteLine("argument error. [folder] [value] [replacement]");
                return;
            }

            DependencyInjector.Setup(new BaseDirectoryAssemblyContainer())
                .RegisterAssemblies<AssemblyInfoBase>()
                .RegisterAssemblies<StaticFileAssemblyInfo>();

            var replacement = DependencyInjector.GetObject<IReplacement>();
            replacement.Execute(args[0], args[1], args[2]);
        }
    }
}