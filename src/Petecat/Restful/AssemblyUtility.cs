using System;
using System.Reflection;

namespace Petecat.Restful
{
    /// <summary>
    /// Assembly utility functions.
    /// </summary>
    [AutoSetupService(typeof(IAssemblyUtility))]
    internal class AssemblyUtility : IAssemblyUtility
    {
        /// <summary>
        /// Static assembly interface.
        /// </summary>
        private IStaticAssembly staticAssembly = null;

        /// <summary>
        /// Static path interface.
        /// </summary>
        private IStaticPath staticPath = null;

        /// <summary>
        /// Initializes a new instance of the AssemblyUtility class.
        /// </summary>
        /// <param name="staticAssembly">Static assembly interface.</param>
        /// <param name="staticPath">Static path interface.</param>
        public AssemblyUtility(IStaticAssembly staticAssembly, IStaticPath staticPath)
        {
            this.staticAssembly = staticAssembly;
        }

        /// <summary>
        /// Load reference assembly handler.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="args">Event args.</param>
        /// <returns>Loaded assembly.</returns>
        public Assembly LoadReferenceAssemblyHandler(object sender, ResolveEventArgs args)
        {
            Assembly result = null;
            AssemblyName assemblyName = new AssemblyName(args.Name);
            if (args != null && args.RequestingAssembly != null && !string.IsNullOrWhiteSpace(args.RequestingAssembly.Location))
            {
                string directory = this.staticPath.GetDirectoryName(args.RequestingAssembly.Location);
                if (!string.IsNullOrWhiteSpace(directory))
                {
                    result = this.staticAssembly.LoadFile(this.staticPath.Combine(new string[]
					{
						directory,
						string.Format("{0}.dll", assemblyName.Name)
					}));
                }
            }
            return result;
        }
    }
}
