using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Petecat.Restful
{
    /// <summary>
    /// Default assemblies loader.
    /// </summary>
    [AutoSetupService(typeof(IAssembliesLoader))]
    internal class DefaultAssemblyLoader : IAssembliesLoader
    {
        /// <summary>
        /// App domain interface.
        /// </summary>
        private readonly ICurrentAppDomain appDomain;

        /// <summary>
        /// Static assembly interface.
        /// </summary>
        private readonly IStaticAssembly staticAssembly;

        /// <summary>
        /// Static path.
        /// </summary>
        private readonly IStaticPath staticPath;

        /// <summary>
        /// Static directory.
        /// </summary>
        private readonly IStaticDirectory staticDirectory;

        /// <summary>
        /// The static configuration manager.
        /// </summary>
        private readonly IStaticConfigurationManager staticConfigurationManager;

        /// <summary>
        /// The assembles' dll file infomation.
        /// </summary>
        private readonly List<AssemblyDllInfo> assemblyDllInfos;

        /// <summary>
        /// Initializes a new instance of the DefaultAssemblyLoader class.
        /// </summary>
        /// <param name="appDomain">App domain interface.</param>
        /// <param name="staticAssembly">Static assembly interface.</param>
        /// <param name="assemblyUtility">Assembly utility.</param>
        /// <param name="staticPath">Static path.</param>
        /// <param name="staticDirectory">Static directory.</param>
        /// <param name="staticConfigurationManager">The static configuration manager.</param>
        public DefaultAssemblyLoader(ICurrentAppDomain appDomain, IStaticAssembly staticAssembly, IAssemblyUtility assemblyUtility, IStaticPath staticPath, IStaticDirectory staticDirectory, IStaticConfigurationManager staticConfigurationManager)
        {
            this.appDomain = appDomain;
            this.staticAssembly = staticAssembly;
            this.staticPath = staticPath;
            this.staticDirectory = staticDirectory;
            this.staticConfigurationManager = staticConfigurationManager;
            this.appDomain.AddAssemblyResolve(new ResolveEventHandler(assemblyUtility.LoadReferenceAssemblyHandler));
            this.assemblyDllInfos = new List<AssemblyDllInfo>();
        }

        /// <summary>
        /// Load application assemblies.
        /// </summary>
        public void LoadAssemblies()
        {
            this.LoadAssemblyInfos(this.GetAppDirectory());
            IEnumerable<AssemblyDllInfo> sortedAssemblies = from f in this.assemblyDllInfos
                                                            group f by f.FileName into g
                                                            select (from f in g
                                                                    orderby f.LastTime descending
                                                                    select f).First<AssemblyDllInfo>();
            List<string> loadedAssemblies = (from item in this.appDomain.GetDomainAssemblies()
                                             where !item.IsDynamic
                                             select Path.GetFileName(item.Location)).ToList<string>();
            foreach (AssemblyDllInfo assembly in sortedAssemblies)
            {
                if (!loadedAssemblies.Contains(assembly.FileName))
                {
                    this.staticAssembly.LoadFile(assembly.FullPath);
                    loadedAssemblies.Add(assembly.FileName);
                }
            }
        }

        /// <summary>
        /// Get app directory.
        /// </summary>
        /// <returns>App directory.</returns>
        private string GetAppDirectory()
        {
            string result;
            if (!string.IsNullOrWhiteSpace(this.appDomain.DynamicDirectory))
            {
                result = this.appDomain.DynamicDirectory;
            }
            else
            {
                result = this.appDomain.BaseDirectory;
            }
            return result;
        }

        /// <summary>
        /// Load Newegg Service Assemblies.
        /// </summary>
        /// <param name="path">Requested path for loading assemblies.</param>
        private void LoadAssemblies(string path)
        {
            if (this.ValidatePath(path))
            {
                IEnumerable<string> assemblies = this.staticDirectory.GetFiles(path, "*.dll");
                if (assemblies.Any<string>())
                {
                    List<string> loadedAssemblies = (from item in this.appDomain.GetDomainAssemblies()
                                                     where !item.IsDynamic
                                                     select item.Location).ToList<string>();
                    foreach (string file in assemblies)
                    {
                        if (!loadedAssemblies.Contains(file))
                        {
                            this.staticAssembly.LoadFile(file);
                            loadedAssemblies.Add(file);
                        }
                    }
                }
                IEnumerable<string> sudDirectories = this.staticDirectory.GetDirectories(path);
                foreach (string subDirectory in sudDirectories)
                {
                    this.LoadAssemblies(subDirectory);
                }
            }
        }

        /// <summary>
        /// Load Newegg Service Assemblies.
        /// </summary>
        /// <param name="path">Requested path for loading assemblies.</param>
        private void LoadAssemblyInfos(string path)
        {
            if (this.ValidatePath(path))
            {
                FileInfo[] assemblies = this.staticDirectory.GetFileInfos(path, "*.dll");
                if (assemblies.Any<FileInfo>())
                {
                    this.assemblyDllInfos.AddRange(from f in assemblies
                                                   select new AssemblyDllInfo
                                                   {
                                                       FileName = f.Name,
                                                       FullPath = f.FullName,
                                                       LastTime = f.LastWriteTime
                                                   });
                }
                IEnumerable<string> sudDirectories = this.staticDirectory.GetDirectories(path);
                foreach (string subDirectory in sudDirectories)
                {
                    this.LoadAssemblyInfos(subDirectory);
                }
            }
        }

        /// <summary>
        /// Validate path.
        /// </summary>
        /// <param name="path">Requested path.</param>
        /// <returns>Validate result.</returns>
        private bool ValidatePath(string path)
        {
            bool result;
            if (string.IsNullOrWhiteSpace(path))
            {
                result = false;
            }
            else
            {
                string currentFolder = this.staticPath.GetFileName(path);
                result = (!string.Equals("obj", currentFolder, StringComparison.OrdinalIgnoreCase) && !this.staticConfigurationManager.GetAppSetting("AssemblyLoaderFilter").Split(new char[]
				{
					';'
				}).Any(new Func<string, bool>(currentFolder.EqualsIgnoreCase)) && !currentFolder.StartsWith("Temp", StringComparison.OrdinalIgnoreCase) && !currentFolder.StartsWith("Tmp", StringComparison.OrdinalIgnoreCase));
            }
            return result;
        }
    }
}
